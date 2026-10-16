using Hafi_Abi_Doğum_Günü.Dtos;
using Hafi_Abi_Doğum_Günü.Models;
using Hafi_Abi_Doğum_Günü.Repositories.Interface;
using Hafi_Abi_Doğum_Günü.Services.Interfaces;

namespace Hafi_Abi_Doğum_Günü.Services.Implementations
{
    public class RsvpService : IRsvpService
    {
        private readonly IRsvpRepository _rsvpRepository;

        public RsvpService(IRsvpRepository rsvpRepository)
        {
            _rsvpRepository = rsvpRepository;
        }

        public async Task<List<Rsvp>> GetAttendingGuestsAsync()
        {
            return await _rsvpRepository.GetAllAttendingAsync();
        }

        public async Task CreateRsvpAsync(RsvpCreateDto dto)
        {
            var rsvp = new Rsvp
            {
                GuestName = dto.GuestName.Trim(),
                IsAttending = dto.IsAttending,
                CreatedAt = DateTime.UtcNow
            };

            await _rsvpRepository.AddAsync(rsvp);
        }
    }
}