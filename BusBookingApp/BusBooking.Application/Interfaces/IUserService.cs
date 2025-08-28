using BusBooking.Application.DTOs;
using BusBooking.Shared.Models;

namespace BusBooking.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> RegisterAsync(RegisterRequestDto dto);
}
