using BusBooking.Domain.Entities;

namespace BusBooking.Infrastructure.Repositories.Interfaces;

public interface IBusRepository
{
    Task<IEnumerable<Bus>> SearchBusesAsync(string from, string to, DateTime date);
    Task<Bus?> GetBusByIdAsync(int id);
    Task<List<Bus>> GetAllAsync();
    Task<int> AddAsync(Bus bus);
}
