using Hafi_Abi_Doğum_Günü.Models;

namespace Hafi_Abi_Doğum_Günü.Repositories.Interface
{
    public interface IRsvpRepository
    {
        Task<List<Rsvp>> GetAllAttendingAsync();
        Task AddAsync(Rsvp rsvp);
    }
}