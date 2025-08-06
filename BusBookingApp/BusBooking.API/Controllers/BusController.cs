using BusBooking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusBooking.API.Controllers;

[ApiController]
[Route("bus")]
public class BusController : ControllerBase
{
    private readonly IBusService _busService;

    public BusController(IBusService busService)
    {
        _busService = busService;
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> Search([FromQuery] string from, [FromQuery] string to, [FromQuery] DateTime date)
    {
        var results = await _busService.SearchBusesAsync(from, to, date);
        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBus(int id)
    {
        var bus = await _busService.GetBusByIdAsync(id);
        if (bus == null)
            return NotFound();

        return Ok(bus);
    }
}
