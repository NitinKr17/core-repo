using BusBooking.Application.DTOs;
using BusBooking.Application.Interfaces;
using BusBooking.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusBooking.API.Controllers;

[ApiController]
[Route(APIConstants.Controller)]
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

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _busService.GetAllAsync();
        return Ok(list);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] BusDto dto)
    {
        var id = await _busService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetBus), new { id }, new { id });
    }
}
