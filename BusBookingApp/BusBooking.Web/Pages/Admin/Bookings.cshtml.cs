using BusBooking.Application.DTOs;
using BusBooking.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace BusBooking.Web.Pages.Admin;

[Authorize(Roles = AppConstants.AdminRole)]
public class BookingsModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private HttpClient Client => _httpClientFactory.CreateClient(AppConstants.BusBookingAPI);

    public BookingsModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty(SupportsGet = true)]
    public int? BusId { get; set; }

    public List<BookingHistoryDto> Bookings { get; private set; } = new();
    public BusDto? FilterBus { get; private set; }

    public async Task OnGetAsync()
    {
        HttpResponseMessage resp;
        if (BusId.HasValue && BusId.Value > 0)
        {
            resp = await Client.GetAsync(string.Format(AppConstants.BookingsByBusEndpoint, BusId.Value));

            var busResp = await Client.GetAsync($"bus/{BusId.Value}");
            if (busResp.IsSuccessStatusCode)
            {
                FilterBus = JsonSerializer.Deserialize<BusDto>(
                    await busResp.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
        else
        {
            resp = await Client.GetAsync(AppConstants.BookingsAllEndpoint);
        }

        if (resp.IsSuccessStatusCode)
        {
            Bookings = JsonSerializer.Deserialize<List<BookingHistoryDto>>(
                await resp.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        else
        {
            TempData[AppConstants.ErrorMessageKey] = AppConstants.FailedLoadBookingsError;
        }
    }
}
