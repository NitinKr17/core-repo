using BusBooking.Application.DTOs;
using BusBooking.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BusBooking.Web.Pages.Home;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<PartialViewResult> OnGetSearchAsync(string from, string to, DateTime date)
    {
        var client = _httpClientFactory.CreateClient("BusBookingAPI");
        var response = await client.GetAsync($"bus/search?from={from}&to={to}&date={date:yyyy-MM-dd}");

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<BusDto>>();
            return Partial("~/Pages/Shared/Partial/_BusSearchResultsPartial.cshtml", data);
        }
        else
        {
            string errorMessage = await response.Content.ReadAsStringAsync();

            ViewData[AppConstants.ErrorMessage] = string.IsNullOrWhiteSpace(errorMessage)
                ? AppConstants.ErrorOccurred
                : errorMessage;

            return Partial("~/Pages/Shared/Partial/_BusSearchResultsPartial.cshtml", Enumerable.Empty<BusDto>());
        }
    }
}
