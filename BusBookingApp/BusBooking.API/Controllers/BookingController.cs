using BusBooking.Application.DTOs;
using BusBooking.Application.Interfaces;
using BusBooking.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusBooking.API.Controllers;

[ApiController]
[Route(APIConstants.Controller)]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBooking([FromBody] BookingDto bookingDto)
    {
        if (bookingDto == null || bookingDto.Passengers == null || !bookingDto.Passengers.Any())
        {
            return BadRequest("Booking and passenger details are required.");
        }

        var bookingId = await _bookingService.CreateBookingAsync(bookingDto);
        return Ok(new { BookingId = bookingId });
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetBookingsByUser(int userId)
        => Ok(await _bookingService.GetBookingsByUserIdAsync(userId));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        return booking is null ? NotFound() : Ok(booking);
    }

    [HttpDelete("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var ok = await _bookingService.CancelBookingAsync(id);
        return ok ? Ok() : BadRequest("Cannot cancel this booking.");
    }

    [HttpPost("{id:int}/passengers")]
    public async Task<IActionResult> AddPassenger(int id, [FromBody] PassengerDto dto)
    {
        var ok = await _bookingService.AddPassengerAsync(id, dto);
        return ok ? Ok() : BadRequest("Cannot add passenger (no seats or trip not in future).");
    }

    [HttpDelete("{id:int}/passengers/{passengerId:int}")]
    public async Task<IActionResult> RemovePassenger(int id, int passengerId)
    {
        var ok = await _bookingService.RemovePassengerAsync(id, passengerId);
        return ok ? Ok() : BadRequest("Cannot remove passenger (trip not in future).");
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _bookingService.GetAllBookingsAsync();
        return Ok(list);
    }

    [HttpGet("bus/{busId:int}")]
    public async Task<IActionResult> GetByBus(int busId)
    {
        var list = await _bookingService.GetBookingsByBusIdAsync(busId);
        return Ok(list);
    }
}
