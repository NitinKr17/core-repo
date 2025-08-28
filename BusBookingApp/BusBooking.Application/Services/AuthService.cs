using BusBooking.Application.DTOs;
using BusBooking.Application.Interfaces;
using BusBooking.Domain.Entities;
using BusBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BusBooking.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher<User> _hasher;

    public AuthService(IAuthRepository authRepository, IPasswordHasher<User> hasher)
    {
        _authRepository = authRepository;
        _hasher = hasher;
    }

    public UserDto? Authenticate(string username, string password)
    {
        var user = _authRepository.GetUserByEmailAsync(username).GetAwaiter().GetResult();
        if (user == null) return null;

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            return null;

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role
        };
    }
}
