using BusBooking.Application.DTOs;

namespace BusBooking.Application.Interfaces;

public interface IBookingService
{
    Task<int> CreateBookingAsync(BookingDto dto);
    Task<List<BookingHistoryDto>> GetBookingsByUserIdAsync(int userId);
    Task<BookingHistoryDto?> GetBookingByIdAsync(int bookingId);
    Task<bool> CancelBookingAsync(int bookingId);
    Task<bool> AddPassengerAsync(int bookingId, PassengerDto passenger);
    Task<bool> RemovePassengerAsync(int bookingId, int passengerId);
    Task<List<BookingHistoryDto>> GetAllBookingsAsync();
    Task<List<BookingHistoryDto>> GetBookingsByBusIdAsync(int busId);
}
