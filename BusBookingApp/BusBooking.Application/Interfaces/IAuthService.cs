using BusBooking.Application.DTOs;

namespace BusBooking.Application.Interfaces;

public interface IAuthService
{
    UserDto? Authenticate(string username, string password);
}
