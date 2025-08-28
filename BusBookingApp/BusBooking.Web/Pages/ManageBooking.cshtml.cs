using BusBooking.Application.DTOs;
using BusBooking.Shared.Constants;
using BusBooking.Web.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace BusBooking.Web.Pages;

[Authorize]
public class ManageBookingModel : BasePageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient? client;
    public ManageBookingModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        client = _httpClientFactory.CreateClient(AppConstants.BusBookingAPI);
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public BookingHistoryDto? Booking { get; set; }

    [BindProperty]
    [Required(ErrorMessage = AppConstants.NameRequiredError)]
    [StringLength(100, ErrorMessage = AppConstants.NameCharatersError)]
    public string PassengerName { get; set; }

    [BindProperty]
    [Required(ErrorMessage = AppConstants.AgeRequiredError)]
    [Range(1, 120, ErrorMessage = AppConstants.InvalidAgeError)]
    public int PassengerAge { get; set; }

    [BindProperty]
    [Required(ErrorMessage = AppConstants.GenderRequiredError)]
    [RegularExpression("Male|Female", ErrorMessage = AppConstants.GenderTypeError)]
    public string PassengerGender { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var resp = await client.GetAsync(string.Format(AppConstants.BookingById, Id));
        if (!resp.IsSuccessStatusCode)
        {
            TempData[AppConstants.ErrorMessageKey] = AppConstants.CouldNotLoadError;
            return RedirectToPage(AppConstants.BookingHistory);
        }
        Booking = JsonSerializer.Deserialize<BookingHistoryDto>(
            await resp.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (Booking?.Bus == null)
        {
            TempData[AppConstants.ErrorMessageKey] = AppConstants.InvalidBookingError;
            return RedirectToPage(AppConstants.BookingHistory);
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        var current = await client.GetFromJsonAsync<BookingHistoryDto>(string.Format(AppConstants.BookingById, Id));
        if (current?.Bus == null || current.Bus.DepartureTime <= DateTime.UtcNow)
            return await BackWithError(AppConstants.FutureBookingError);

        if (current.Bus.AvailableSeats <= 0)
            return await BackWithError(AppConstants.AvailableSeatsError);

        var payload = new PassengerDto { Name = PassengerName, Age = PassengerAge, Gender = PassengerGender };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, AppConstants.JsonContentType);

        var resp = await client.PostAsync(string.Format(AppConstants.BookingAddPassenger, Id), content);

        TempData[resp.IsSuccessStatusCode ? AppConstants.SuccessMessageKey : AppConstants.ErrorMessageKey] =
            resp.IsSuccessStatusCode ? AppConstants.PassengerAdded : AppConstants.FailedPassengerError;

        return RedirectToPage(new { id = Id });
    }

    public async Task<IActionResult> OnPostRemoveAsync(int passengerId, bool cancelIfLast)
    {
        var current = await client.GetFromJsonAsync<BookingHistoryDto>(string.Format(AppConstants.BookingById, Id));
        if (current?.Bus == null || current.Bus.DepartureTime <= DateTime.UtcNow)
            return await BackWithError(AppConstants.FutureBookingError);

        var count = current!.Passengers?.Count ?? 0;
        if (cancelIfLast || count <= 1)
        {
            return await CancelEntireBookingAsync();
        }

        return await RemovePassengerAsync(passengerId);
    }

    public async Task<IActionResult> OnPostCancelAsync(int bookingId)
    {
        var current = await client.GetFromJsonAsync<BookingHistoryDto>(string.Format(AppConstants.BookingById, Id));
        if (current?.Bus == null || current.Bus.DepartureTime <= DateTime.UtcNow)
            return await BackWithError(AppConstants.FutureBookingError);

        return await CancelEntireBookingAsync();
    }

    private async Task<IActionResult> CancelEntireBookingAsync()
    {
        var resp = await client.DeleteAsync(string.Format(AppConstants.BookingCancel, Id));
        TempData[resp.IsSuccessStatusCode ? AppConstants.SuccessMessageKey : AppConstants.ErrorMessageKey] =
            resp.IsSuccessStatusCode ? AppConstants.BookingCancelledMessage : AppConstants.FailedCancelError;

        return RedirectToPage(new { id = Id });
    }

    private async Task<IActionResult> RemovePassengerAsync(int passengerId)
    {
        var resp = await client.DeleteAsync(string.Format(AppConstants.BookingRemovePassenger, Id, passengerId));
        TempData[resp.IsSuccessStatusCode ? AppConstants.SuccessMessageKey : AppConstants.ErrorMessageKey] =
            resp.IsSuccessStatusCode ? AppConstants.PassengerRemoved : AppConstants.PassengerFailedError;

        return RedirectToPage(new { id = Id });
    }

    private async Task<IActionResult> BackWithError(string message)
    {
        TempData[AppConstants.ErrorMessageKey] = message;
        return await OnGetAsync();
    }
}
