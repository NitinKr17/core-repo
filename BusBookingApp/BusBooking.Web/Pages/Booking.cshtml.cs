using BusBooking.Application.DTOs;
using BusBooking.Shared.Constants;
using BusBooking.Web.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace BusBooking.Web.Pages;

[Authorize]
public class BookingModel : BasePageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BookingModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty]
    public int BusId { get; set; }

    [BindProperty]
    public string PassengersJson { get; set; } = string.Empty;

    public void OnGet(int busId)
    {
        BusId = busId;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(PassengersJson))
        {
            TempData[AppConstants.ErrorMessageKey] = AppConstants.NoPassengersError;
            return Page();
        }

        List<PassengerDto> passengers;
        try
        {
            passengers = JsonSerializer.Deserialize<List<PassengerDto>>(PassengersJson) ?? new();
        }
        catch
        {
            TempData[AppConstants.ErrorMessageKey] = AppConstants.InvalidPassengerDataError;
            return Page();
        }

        var userIdClaim = User.FindFirst(AppConstants.UserIdClaimType);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            TempData[AppConstants.ErrorMessageKey] = AppConstants.UserClaimError;
            return Page();
        }

        var bookingDto = new BookingDto
        {
            UserId = userId,
            BusId = BusId,
            Passengers = passengers
        };

        var client = _httpClientFactory.CreateClient(AppConstants.BusBookingAPI);

        var content = new StringContent(JsonSerializer.Serialize(bookingDto), Encoding.UTF8, AppConstants.JsonContentType);
        var response = await client.PostAsync(AppConstants.BookingCreateEndpoint, content);

        if (response.IsSuccessStatusCode)
        {
            TempData[AppConstants.SuccessMessageKey] = AppConstants.BookingSuccessMessage;
        }
        else
        {
            TempData[AppConstants.ErrorMessageKey] = AppConstants.BookingFailedError;
        }

        return Page();
    }
}
