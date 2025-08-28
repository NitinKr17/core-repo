using BusBooking.Application.DTOs;
using BusBooking.Application.Interfaces;
using BusBooking.Domain.Entities;
using BusBooking.Infrastructure.Repositories.Interfaces;
using BusBooking.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace BusBooking.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _UserRepository;
    private readonly IPasswordHasher<User> _hasher;

    public UserService(IUserRepository UserRepository, IPasswordHasher<User> hasher)
    {
        _UserRepository = UserRepository;
        _hasher = hasher;
    }

    public async Task<UserDto?> RegisterAsync(RegisterRequestDto dto)
    {
        var existing = await _UserRepository.GetByEmailAsync(dto.Email);
        if (existing != null) return null; 

        var user = new User
        {
            Email = dto.Email,
            FullName = dto.FullName,
            Role = dto.Role
        };

        user.PasswordHash = _hasher.HashPassword(user, dto.Password);

        var created = await _UserRepository.AddAsync(user);

        return new UserDto
        {
            Id = created.Id,
            Email = created.Email,
            FullName = created.FullName,
            Role = created.Role
        };
    }
}
