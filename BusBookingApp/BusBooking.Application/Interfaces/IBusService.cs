using BusBooking.Application.DTOs;

namespace BusBooking.Application.Interfaces;

public interface IBusService
{
    Task<IEnumerable<BusDto>> SearchBusesAsync(string from, string to, DateTime date);
    Task<BusDto?> GetBusByIdAsync(int id);
    Task<IEnumerable<BusDto>> GetAllAsync();
    Task<int> CreateAsync(BusDto dto);
}
