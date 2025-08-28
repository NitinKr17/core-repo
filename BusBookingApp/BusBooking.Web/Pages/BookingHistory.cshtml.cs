using BusBooking.Application.DTOs;
using BusBooking.Shared.Constants;
using BusBooking.Web.Pages.Shared;
using Microsoft.AspNetCore.Authorization;

namespace BusBooking.Web.Pages;

[Authorize]
public class BookingHistoryModel : BasePageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BookingHistoryModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public List<BookingHistoryDto> Bookings { get; set; } = new();

    public async Task OnGetAsync()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return;

        var client = _httpClientFactory.CreateClient(AppConstants.BusBookingAPI);

        var userIdClaim = User.FindFirst(AppConstants.UserIdClaimType);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            TempData[AppConstants.ErrorMessageKey] = AppConstants.UserClaimError;
        }
        else
        {
            var response = await client.GetAsync(string.Format(AppConstants.BookingHistoryEndpoint, userId));
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<BookingHistoryDto>>();
                if (result != null)
                    Bookings = result;
            }
        }
    }
}
