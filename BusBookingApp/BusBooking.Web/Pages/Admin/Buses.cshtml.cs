using BusBooking.Application.DTOs;
using BusBooking.Shared.Attributes;
using BusBooking.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace BusBooking.Web.Pages.Admin;

[Authorize(Roles = AppConstants.AdminRole)]
public class BusesModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient? client;
    public BusesModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        client = _httpClientFactory.CreateClient(AppConstants.BusBookingAPI);
    }

    public List<BusDto> Buses { get; private set; } = new();

    [BindProperty]
    [Required(ErrorMessage = AppConstants.OperatorRequiredError)]
    [StringLength(100, ErrorMessage = AppConstants.OperatorCharatersError)]
    public string OperatorName { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = AppConstants.FromRequiredError)]
    [StringLength(100, ErrorMessage = AppConstants.FromLengthError)]
    public string Source { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = AppConstants.ToRequiredError)]
    [StringLength(100, ErrorMessage = AppConstants.ToLengthError)]
    public string Destination { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = AppConstants.DepartureRequiredError)]
    public DateTime DepartureTime { get; set; } = DateTime.UtcNow.AddDays(1);

    [BindProperty]
    [Required(ErrorMessage = AppConstants.ArrivalRequiredError)]
    public DateTime ArrivalTime { get; set; } = DateTime.UtcNow.AddDays(1).AddHours(3);

    [BindProperty]
    [Required(ErrorMessage = AppConstants.TotalSeatsRequiredError)]
    [Range(1, 200, ErrorMessage = AppConstants.SeatsInvalidError)]
    public int TotalSeats { get; set; } = 40;

    [BindProperty]
    [Required(ErrorMessage = AppConstants.AvailableSeatsRequiredError)]
    [Range(0, 200, ErrorMessage = AppConstants.SeatsInvalidError)]
    [NotGreaterThan(nameof(TotalSeats), ErrorMessage = AppConstants.AvailableSeatsGreaterError)]
    public int AvailableSeats { get; set; } = 40;

    public async Task OnGetAsync()
    {
        var resp = await client.GetAsync(AppConstants.BusListEndpoint);

        if (resp.IsSuccessStatusCode)
        {
            var json = await resp.Content.ReadAsStringAsync();
            Buses = JsonSerializer.Deserialize<List<BusDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<BusDto>();
        }
        else
        {
            TempData[AppConstants.ErrorMessageKey] = AppConstants.ErrorOccurred;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        var dto = new BusDto
        {
            OperatorName = OperatorName,
            Source = Source,
            Destination = Destination,
            DepartureTime = DepartureTime,
            ArrivalTime = ArrivalTime,
            TotalSeats = TotalSeats,
            AvailableSeats = AvailableSeats
        };

        var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, AppConstants.JsonContentType);
        var resp = await client.PostAsync(AppConstants.BusCreateEndpoint, content);

        if (resp.IsSuccessStatusCode)
        {
            TempData[AppConstants.SuccessMessageKey] = AppConstants.BusCreatedMessage;
            return RedirectToPage();
        }

        TempData[AppConstants.ErrorMessageKey] = AppConstants.BusCreateFailedError;
        await OnGetAsync();
        return Page();
    }
}
