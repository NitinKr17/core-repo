using BusBooking.Domain.Entities;
using BusBooking.Infrastructure.Data;
using BusBooking.Infrastructure.Repositories.Interfaces;
using BusBooking.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BusBookingDbContext _context;

    public BookingRepository(BusBookingDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddBookingAsync(Booking booking)
    {
        Bus? bus = await _context.Buses.FirstOrDefaultAsync(b => b.Id == booking.BusId);
        if (bus == null)
        {
            throw new InvalidOperationException(string.Format(AppConstants.BusNotFound, booking.BusId));
        }
        if (bus.AvailableSeats < booking.NumberOfSeats)
        {
            throw new InvalidOperationException(AppConstants.AvailableSeatsError);
        }

        bus.AvailableSeats -= booking.NumberOfSeats;

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return booking.Id;
    }

    public async Task<List<Booking>> GetBookingsByUserIdAsync(int userId) => await _context.Bookings
            .Include(b => b.Bus)
            .Include(b => b.Passengers)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();

    public async Task<Booking?> GetBookingByIdAsync(int id) =>
        await _context.Bookings
            .Include(b => b.Bus)
            .Include(b => b.Passengers)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<bool> CancelBookingAsync(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Bus)
            .Include(b => b.Passengers)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
            throw new InvalidOperationException(string.Format(APIConstants.BookingNotFound, id));

        if (booking.Bus == null)
            throw new InvalidOperationException(string.Format(APIConstants.BusNotFound, booking.BusId));

        if (booking.Bus.DepartureTime <= DateTime.UtcNow)
            throw new InvalidOperationException(APIConstants.PastJourneyModifyError);

        booking.Bus.AvailableSeats += booking.NumberOfSeats;

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddPassengerAsync(int bookingId, Passenger passenger)
    {
        var booking = await _context.Bookings
            .Include(b => b.Bus)
            .Include(b => b.Passengers)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking == null)
            throw new InvalidOperationException(string.Format(APIConstants.BookingNotFound, bookingId));

        if (booking.Bus == null)
            throw new InvalidOperationException(string.Format(APIConstants.BusNotFound, booking.BusId));

        if (booking.Bus.DepartureTime <= DateTime.UtcNow)
            throw new InvalidOperationException(APIConstants.PastJourneyModifyError);

        if (booking.Bus.AvailableSeats <= 0)
            throw new InvalidOperationException(APIConstants.AvailableSeatsError);

        booking.Bus.AvailableSeats -= 1;
        booking.NumberOfSeats += 1;
        booking.Passengers ??= new List<Passenger>();
        booking.Passengers.Add(passenger);

        await _context.SaveChangesAsync(); 
        return true;
    }

    public async Task<bool> RemovePassengerAsync(int bookingId, int passengerId)
    {
        var booking = await _context.Bookings
            .Include(b => b.Bus)
            .Include(b => b.Passengers)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking == null)
            throw new InvalidOperationException(string.Format(APIConstants.BookingNotFound, bookingId));

        if (booking.Bus == null)
            throw new InvalidOperationException(string.Format(APIConstants.BusNotFound, booking.BusId));

        if (booking.Bus.DepartureTime <= DateTime.UtcNow)
            throw new InvalidOperationException(APIConstants.PastJourneyModifyError);

        var passenger = booking.Passengers?.FirstOrDefault(p => p.Id == passengerId);
        if (passenger == null)
            throw new InvalidOperationException(string.Format(APIConstants.PassengerNotFound, passengerId));

        booking.Passengers!.Remove(passenger);
        booking.NumberOfSeats = Math.Max(0, booking.NumberOfSeats - 1);
        booking.Bus.AvailableSeats += 1;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Booking>> GetAllBookingsAsync() =>
        await _context.Bookings
            .Include(b => b.Bus)
            .Include(b => b.Passengers)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();

    public async Task<List<Booking>> GetBookingsByBusIdAsync(int busId) =>
        await _context.Bookings
            .Include(b => b.Bus)
            .Include(b => b.Passengers)
            .Where(b => b.BusId == busId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
}
