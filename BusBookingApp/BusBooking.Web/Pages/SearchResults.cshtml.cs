using BusBooking.Application.DTOs;
using BusBooking.Web.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BusBooking.Web.Pages.Home;

public class SearchResultsModel : BasePageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SearchResultsModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public List<BusDto> Buses { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string From { get; set; }

    [BindProperty(SupportsGet = true)]
    public string To { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime Date { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient("BusBookingAPI");

        var response = await client.GetAsync($"bus/search?from={From}&to={To}&date={Date:yyyy-MM-dd}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            Buses = JsonSerializer.Deserialize<List<BusDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<BusDto>();
        }

        return Page();
    }
}
