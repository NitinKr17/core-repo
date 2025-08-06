using BusBooking.Application.DTOs;
using BusBooking.Application.Interfaces;
using BusBooking.Infrastructure.Repositories.Interfaces;

namespace BusBooking.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public UserDto? Authenticate(string username, string password)
    {
        var userTask = _authRepository.GetUserByEmailAsync(username);
        userTask.Wait();
        var user = userTask.Result;

        if (user == null || user.PasswordHash != password)
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
