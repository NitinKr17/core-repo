using BusBooking.Application.DTOs;
using BusBooking.Application.Interfaces;
using BusBooking.Domain.Entities;
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

    public async Task<IEnumerable<BusDto>> GetAllAsync()
    {
        var buses = await _busRepository.GetAllAsync();
        return buses.Select(Map);
    }

    public async Task<int> CreateAsync(BusDto dto)
    {
        if (dto.DepartureTime >= dto.ArrivalTime)
            throw new ArgumentException("Arrival must be after departure.");

        if (dto.TotalSeats <= 0)
            throw new ArgumentException("Total seats must be positive.");

        var bus = new Bus
        {
            OperatorName = dto.OperatorName.Trim(),
            Source = dto.Source.Trim(),
            Destination = dto.Destination.Trim(),
            DepartureTime = dto.DepartureTime,
            ArrivalTime = dto.ArrivalTime,
            TotalSeats = dto.TotalSeats,
            AvailableSeats = dto.AvailableSeats
        };

        if (bus.AvailableSeats < 0 || bus.AvailableSeats > bus.TotalSeats)
            throw new ArgumentException("Available seats must be between 0 and Total seats.");

        return await _busRepository.AddAsync(bus);
    }

    private static BusDto Map(Bus b) => new()
    {
        Id = b.Id,
        OperatorName = b.OperatorName,
        Source = b.Source,
        Destination = b.Destination,
        DepartureTime = b.DepartureTime,
        ArrivalTime = b.ArrivalTime,
        TotalSeats = b.TotalSeats,
        AvailableSeats = b.AvailableSeats
    };
}
