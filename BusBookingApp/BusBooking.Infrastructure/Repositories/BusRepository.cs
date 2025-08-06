using BusBooking.Domain.Entities;
using BusBooking.Infrastructure.Data;
using BusBooking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Infrastructure.Repositories;

public class BusRepository : IBusRepository
{
    private readonly BusBookingDbContext _context;

    public BusRepository(BusBookingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Bus>> SearchBusesAsync(string from, string to, DateTime date)
    {
        return await _context.Buses
            .Where(b =>
            b.Source.ToLower() == from.ToLower() &&
            b.Destination.ToLower() == to.ToLower() &&
            EF.Functions.DateDiffDay(b.DepartureTime, date) == 0)
        .ToListAsync();
    }

    public async Task<Bus?> GetBusByIdAsync(int id)
    {
        return await _context.Buses.FirstOrDefaultAsync(b => b.Id == id);
    }
}
