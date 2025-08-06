using BusBooking.Domain.Entities;
using BusBooking.Infrastructure.Data;
using BusBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly BusBookingDbContext _context;

    public AuthRepository(BusBookingDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
