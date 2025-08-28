using BusBooking.Shared.Constants;
using BusBooking.Shared.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BusBooking.Web.Pages;

[AllowAnonymous]
public class LoginModel : PageModel
{
    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    private readonly IHttpClientFactory _httpClientFactory;

    public LoginModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var payload = JsonSerializer.Serialize(new { Email, Password });
        var content = new StringContent(payload, Encoding.UTF8, AppConstants.JsonContentType);

        var client = _httpClientFactory.CreateClient(AppConstants.BusBookingAPI);
        var response = await client.PostAsync(AppConstants.AuthLogin, content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<JwtResponse>(result)?.Token;

            if (!string.IsNullOrEmpty(token))
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(token);

                var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, AppConstants.JwtClaim);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);

                HttpContext.Session.SetString(AppConstants.JwtToken, token);

                var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    return Redirect(ReturnUrl);

                return role == AppConstants.AdminRole ? RedirectToPage(AppConstants.AdminBuses) : RedirectToPage(AppConstants.Home);
            }
        }

        ModelState.AddModelError("", AppConstants.InvalidCredentials);
        return Page();
    }
}
