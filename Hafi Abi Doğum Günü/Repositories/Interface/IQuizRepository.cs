using Hafi_Abi_Doğum_Günü.Models;

namespace Hafi_Abi_Doğum_Günü.Repositories.Interface
{
    public interface IQuizRepository
    {
        Task<List<Question>> GetAllQuestionsOrderedAsync();
        Task<List<QuizResult>> GetTopScoresAsync(int count = 10);
        Task AddResultAsync(QuizResult result);
    }
}