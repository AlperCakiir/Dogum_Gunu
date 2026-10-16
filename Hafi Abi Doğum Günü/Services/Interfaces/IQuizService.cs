using Hafi_Abi_Doğum_Günü.Models;

namespace Hafi_Abi_Doğum_Günü.Services.Interfaces
{
    public interface IQuizService
    {
        Task<List<Question>> GetQuizQuestionsAsync();
        Task<List<QuizResult>> GetLeaderboardAsync();
        Task SavePlayerScoreAsync(string playerName, string playerTitle, int score);
    }
}   