using Hafi_Abi_Doğum_Günü.Dtos;
using Hafi_Abi_Doğum_Günü.Models;

namespace Hafi_Abi_Doğum_Günü.Services.Interfaces
{
    public interface IRsvpService
    {
        Task<List<Rsvp>> GetAttendingGuestsAsync();
        Task CreateRsvpAsync(RsvpCreateDto dto);
    }
}