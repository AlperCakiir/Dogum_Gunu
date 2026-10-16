using Hafi_Abi_Doğum_Günü.Data;
using Hafi_Abi_Doğum_Günü.Models;
using Hafi_Abi_Doğum_Günü.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hafi_Abi_Doğum_Günü.Repositories.EfCore
{
    public class RsvpRepository : IRsvpRepository
    {
        private readonly AppDbContext _context;

        public RsvpRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rsvp>> GetAllAttendingAsync()
        {
            return await _context.Rsvps
                .Where(r => r.IsAttending)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Rsvp rsvp)
        {
            await _context.Rsvps.AddAsync(rsvp);
            await _context.SaveChangesAsync();
        }
    }
}