using BusBooking.Application.DTOs;
using BusBooking.Application.Interfaces;
using BusBooking.Infrastructure.Repositories.Interfaces;

namespace BusBooking.Application.Services;

public class BusService : IBusService
{
    private readonly IBusRepository _busRepository;

    public BusService(IBusRepository busRepository)
    {
        _busRepository = busRepository;
    }

    public async Task<IEnumerable<BusDto>> SearchBusesAsync(string from, string to, DateTime date)
    {
        var buses = await _busRepository.SearchBusesAsync(from, to, date);
        return buses.Select(b => new BusDto
        {
            Id = b.Id,
            OperatorName = b.OperatorName,
            Source = b.Source,
            Destination = b.Destination,
            DepartureTime = b.DepartureTime,
            ArrivalTime = b.ArrivalTime,
            AvailableSeats = b.AvailableSeats,
            TotalSeats = b.TotalSeats
        });
    }

    public async Task<BusDto?> GetBusByIdAsync(int id)
    {
        var bus = await _busRepository.GetBusByIdAsync(id);
        if (bus == null) return null;

        return new BusDto
        {
            Id = bus.Id,
            OperatorName = bus.OperatorName,
            Source = bus.Source,
            Destination = bus.Destination,
            DepartureTime = bus.DepartureTime,
            ArrivalTime = bus.ArrivalTime,
            AvailableSeats = bus.AvailableSeats,
            TotalSeats = bus.TotalSeats
        };
    }
}
