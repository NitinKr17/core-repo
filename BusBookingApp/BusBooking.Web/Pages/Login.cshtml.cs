using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BusBooking.Web.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    private readonly IHttpClientFactory _httpClientFactory;

    public LoginModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var payload = JsonSerializer.Serialize(new { Email, Password });
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        var client = _httpClientFactory.CreateClient("BusBookingAPI");
        var response = await client.PostAsync("auth/login", content);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<JwtResponse>(result)?.Token;
            HttpContext.Session.SetString("JwtToken", token);
            return RedirectToPage("Home/Index");
        }

        ModelState.AddModelError("", "Invalid credentials");
        return Page();
    }

    private class JwtResponse
    {
        [JsonPropertyName("token")]
        public required string Token { get; set; }
    }
}
