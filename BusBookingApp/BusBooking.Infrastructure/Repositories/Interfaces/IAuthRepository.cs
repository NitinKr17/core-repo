using BusBooking.Domain.Entities;

namespace BusBooking.Infrastructure.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetUserByEmailAsync(string email);
}
