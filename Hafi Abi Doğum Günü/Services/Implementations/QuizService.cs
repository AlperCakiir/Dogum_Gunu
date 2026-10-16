using Hafi_Abi_Doğum_Günü.Models;
using Hafi_Abi_Doğum_Günü.Repositories.Interface;
using Hafi_Abi_Doğum_Günü.Services.Interfaces;

namespace Hafi_Abi_Doğum_Günü.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<List<Question>> GetQuizQuestionsAsync()
        {
            return await _quizRepository.GetAllQuestionsOrderedAsync();
        }

        public async Task<List<QuizResult>> GetLeaderboardAsync()
        {
            return await _quizRepository.GetTopScoresAsync(10);
        }

        public async Task SavePlayerScoreAsync(string playerName, string playerTitle, int score)
        {
            var result = new QuizResult
            {
                PlayerName = playerName,
                PlayerTitle = playerTitle,
                Score = score,
                PlayedAt = DateTime.UtcNow
            };

            await _quizRepository.AddResultAsync(result);
        }
    }
}