using BusBooking.Domain.Entities;
using BusBooking.Infrastructure.Data;
using BusBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BusBookingDbContext _context;
    public UserRepository(BusBookingDbContext context) => _context = context;

    public Task<User?> GetByEmailAsync(string email) =>
        _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
