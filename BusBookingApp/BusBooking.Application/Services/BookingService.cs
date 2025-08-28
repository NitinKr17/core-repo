using BusBooking.Application.DTOs;
using BusBooking.Application.Interfaces;
using BusBooking.Domain.Entities;
using BusBooking.Infrastructure.Repositories.Interfaces;

namespace BusBooking.Application.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<int> CreateBookingAsync(BookingDto dto)
    {
        var booking = new Booking
        {
            UserId = dto.UserId,
            BusId = dto.BusId,
            BookingDate = DateTime.UtcNow,
            NumberOfSeats = dto.Passengers.Count,
            Passengers = dto.Passengers.Select(p => new Passenger
            {
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender
            }).ToList()
        };

        return await _bookingRepository.AddBookingAsync(booking);
    }

    public async Task<List<BookingHistoryDto>> GetBookingsByUserIdAsync(int userId)
    {
        var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
        return Map(bookings);
    }

    public async Task<BookingHistoryDto?> GetBookingByIdAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
        if (booking == null) return null;

        return Map(booking);
    }

    public async Task<bool> CancelBookingAsync(int bookingId)
        => await _bookingRepository.CancelBookingAsync(bookingId);

    public async Task<bool> AddPassengerAsync(int bookingId, PassengerDto passenger)
    {
        var p = new Passenger
        {
            Name = passenger.Name,
            Age = passenger.Age,
            Gender = passenger.Gender
        };

        return await _bookingRepository.AddPassengerAsync(bookingId, p);
    }

    public Task<bool> RemovePassengerAsync(int bookingId, int passengerId)
        => _bookingRepository.RemovePassengerAsync(bookingId, passengerId);

    public async Task<List<BookingHistoryDto>> GetAllBookingsAsync()
    {
        var bookings = await _bookingRepository.GetAllBookingsAsync();
        return Map(bookings);
    }

    public async Task<List<BookingHistoryDto>> GetBookingsByBusIdAsync(int busId)
    {
        var bookings = await _bookingRepository.GetBookingsByBusIdAsync(busId);
        return Map(bookings);
    }

    private static List<BookingHistoryDto> Map(IEnumerable<Booking> bookings)
        => bookings.Select(b => new BookingHistoryDto
        {
            UserId = b.UserId,
            Id = b.Id,
            BookingDate = b.BookingDate,
            NumberOfSeats = b.NumberOfSeats,
            Bus = b.Bus == null ? new() : new BusDto
            {
                Id = b.Bus.Id,
                OperatorName = b.Bus.OperatorName,
                Source = b.Bus.Source,
                Destination = b.Bus.Destination,
                DepartureTime = b.Bus.DepartureTime,
                ArrivalTime = b.Bus.ArrivalTime,
                TotalSeats = b.Bus.TotalSeats,
                AvailableSeats = b.Bus.AvailableSeats
            },
            Passengers = b.Passengers?.Select(p => new PassengerDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender
            }).ToList() ?? new()
        }).ToList();

    private static BookingHistoryDto Map(Booking booking)
        => new BookingHistoryDto
        {
            UserId = booking.UserId,
            Id = booking.Id,
            BookingDate = booking.BookingDate,
            NumberOfSeats = booking.NumberOfSeats,
            Bus = booking.Bus == null ? new() : new BusDto
            {
                Id = booking.Bus.Id,
                OperatorName = booking.Bus.OperatorName,
                Source = booking.Bus.Source,
                Destination = booking.Bus.Destination,
                DepartureTime = booking.Bus.DepartureTime,
                ArrivalTime = booking.Bus.ArrivalTime,
                TotalSeats = booking.Bus.TotalSeats,
                AvailableSeats = booking.Bus.AvailableSeats
            },
            Passengers = booking.Passengers?.Select(p => new PassengerDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender
            }).ToList() ?? new()
        };
}
