using Hafi_Abi_Doğum_Günü.Models;
using Hafi_Abi_Doğum_Günü.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Hafi_Abi_Doğum_Günü.Hubs
{
    public class LobbyRoom
    {
        public string LobbyCode { get; set; } = string.Empty;
        public string TargetConnectionId { get; set; } = string.Empty;
        public List<LobbyPlayer> Players { get; set; } = new List<LobbyPlayer>();
        public List<Question> Questions { get; set; } = new List<Question>();
        public int CurrentQuestionIndex { get; set; } = -1;
    }

    public class LobbyHub : Hub
    {
        private readonly IQuizService _quizService;
        private static readonly Dictionary<string, LobbyRoom> Lobbies = new Dictionary<string, LobbyRoom>();

        public LobbyHub(IQuizService quizService)
        {
            _quizService = quizService;
        }

        public async Task JoinLobbyWithCode(string lobbyCode, string playerName, bool isHost)
        {
            lobbyCode = lobbyCode.ToUpper();
            if (!Lobbies.ContainsKey(lobbyCode))
            {
                Lobbies[lobbyCode] = new LobbyRoom { LobbyCode = lobbyCode };
            }

            var lobby = Lobbies[lobbyCode];
            if (!lobby.Players.Any(p => p.ConnectionId == Context.ConnectionId))
            {
                lobby.Players.Add(new LobbyPlayer
                {
                    ConnectionId = Context.ConnectionId,
                    Name = playerName,
                    Score = 0,
                    IsHost = isHost || lobby.Players.Count == 0
                });
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            await Clients.Group(lobbyCode).SendAsync("UpdatePlayerList", lobby.Players);
        }

        public async Task StartGameWithTarget(string lobbyCode, string targetConnId)
        {
            lobbyCode = lobbyCode.ToUpper();
            if (!Lobbies.ContainsKey(lobbyCode)) return;

            var lobby = Lobbies[lobbyCode];
            lobby.TargetConnectionId = targetConnId;
            lobby.Questions = await _quizService.GetQuizQuestionsAsync();
            lobby.CurrentQuestionIndex = 0;

            foreach (var p in lobby.Players)
            {
                p.Score = 0;
                p.IsTarget = (p.ConnectionId == targetConnId);
                p.CurrentAnswer = string.Empty;
            }

            await Clients.Group(lobbyCode).SendAsync("UpdatePlayerList", lobby.Players);
            await SendCurrentQuestion(lobby);
        }

        public async Task SubmitAnswerCode(string lobbyCode, string selectedOption)
        {
            lobbyCode = lobbyCode.ToUpper();
            if (!Lobbies.ContainsKey(lobbyCode)) return;

            var lobby = Lobbies[lobbyCode];
            var player = lobby.Players.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
            if (player != null)
            {
                player.CurrentAnswer = selectedOption;
            }
        }

        public async Task EvaluateRoundAndScore(string lobbyCode, string guessedPlayerConnId)
        {
            lobbyCode = lobbyCode.ToUpper();
            if (!Lobbies.ContainsKey(lobbyCode)) return;

            var lobby = Lobbies[lobbyCode];
            var targetPlayer = lobby.Players.FirstOrDefault(p => p.ConnectionId == lobby.TargetConnectionId);
            if (targetPlayer == null) return;

            string targetAnswer = string.IsNullOrEmpty(targetPlayer.CurrentAnswer) ? "A" : targetPlayer.CurrentAnswer;

            // 1) Hedef kişinin cevabını doğru bilen herkese +10 puan
            foreach (var p in lobby.Players)
            {
                if (p.ConnectionId != targetPlayer.ConnectionId && p.CurrentAnswer == targetAnswer)
                {
                    p.Score += 10;
                }
            }

            // 2) Hedef kişinin "Beni kim doğru bildi?" tahmini
            string summaryMsg = $"🎯 {targetPlayer.Name} '{targetAnswer}' seçmişti! ";
            if (guessedPlayerConnId != "NONE")
            {
                var guessedPerson = lobby.Players.FirstOrDefault(p => p.ConnectionId == guessedPlayerConnId);
                if (guessedPerson != null)
                {
                    if (guessedPerson.CurrentAnswer == targetAnswer)
                    {
                        // Doğru bildiyse: Hedef +10 ve bildiğinden +5 (toplam +15) alır!
                        targetPlayer.Score += 15;
                        summaryMsg += $"🎉 {targetPlayer.Name}, {guessedPerson.Name}'in doğru bileceğini bildi (+15 Puan)!";
                    }
                    else
                    {
                        // Yanlış tahmin ettiyse: -10 puan ceza!
                        targetPlayer.Score -= 10;
                        summaryMsg += $"❌ {targetPlayer.Name}, {guessedPerson.Name} üzerinde yanlış tahmin yaptı (-10 Puan)!";
                    }
                }
            }

            await Clients.Group(lobbyCode).SendAsync("UpdatePlayerList", lobby.Players);
            await Clients.Group(lobbyCode).SendAsync("RoundResultAnnouncement", summaryMsg);

            await Task.Delay(3500);

            lobby.CurrentQuestionIndex++;
            if (lobby.CurrentQuestionIndex < lobby.Questions.Count)
            {
                await SendCurrentQuestion(lobby);
            }
            else
            {
                var sorted = lobby.Players.OrderByDescending(p => p.Score).ToList();
                await Clients.Group(lobbyCode).SendAsync("GameOver", sorted);
            }
        }

        private async Task SendCurrentQuestion(LobbyRoom lobby)
        {
            if (lobby.CurrentQuestionIndex < lobby.Questions.Count)
            {
                var q = lobby.Questions[lobby.CurrentQuestionIndex];
                var targetPlayer = lobby.Players.FirstOrDefault(p => p.ConnectionId == lobby.TargetConnectionId);
                string targetName = targetPlayer?.Name ?? "Hedef Kişi";

                foreach (var p in lobby.Players) p.CurrentAnswer = string.Empty;

                // Sadece hedef kişinin adını ve hazırladığımız genel soruyu birleştirir (Örn: "Alper ıssız bir adaya düşse...")
                string dynamicQuestionText = $"{targetName} {q.QuestionText}";

                await Clients.Group(lobby.LobbyCode).SendAsync("ReceiveQuestion", new
                {
                    questionNumber = lobby.CurrentQuestionIndex + 1,
                    totalQuestions = lobby.Questions.Count,
                    questionText = dynamicQuestionText,
                    optionA = q.OptionA,
                    optionB = q.OptionB,
                    optionC = q.OptionC,
                    optionD = q.OptionD,
                    targetConnectionId = lobby.TargetConnectionId,
                    targetName = targetName,
                    players = lobby.Players
                });
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (var lobby in Lobbies.Values)
            {
                var p = lobby.Players.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                if (p != null)
                {
                    lobby.Players.Remove(p);
                    await Clients.Group(lobby.LobbyCode).SendAsync("UpdatePlayerList", lobby.Players);
                    break;
                }
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}