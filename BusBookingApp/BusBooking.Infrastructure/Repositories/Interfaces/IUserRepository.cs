using BusBooking.Domain.Entities;

namespace BusBooking.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User> AddAsync(User user);
}
