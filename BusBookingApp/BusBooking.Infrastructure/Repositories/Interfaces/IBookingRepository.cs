using BusBooking.Domain.Entities;

namespace BusBooking.Infrastructure.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<int> AddBookingAsync(Booking booking);
    Task<List<Booking>> GetBookingsByUserIdAsync(int userId);
    Task<Booking?> GetBookingByIdAsync(int id);
    Task<bool> CancelBookingAsync(int id);
    Task<bool> AddPassengerAsync(int bookingId, Passenger passenger);
    Task<bool> RemovePassengerAsync(int bookingId, int passengerId);
    Task<List<Booking>> GetAllBookingsAsync();
    Task<List<Booking>> GetBookingsByBusIdAsync(int busId);
}
