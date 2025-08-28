using BusBooking.Domain.Enums;
using BusBooking.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace BusBooking.Web.Pages;

public class RegisterModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    public RegisterModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    [BindProperty, Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [BindProperty, Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BindProperty, Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [BindProperty, Compare(nameof(Password), ErrorMessage = AppConstants.PasswordMatchError)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [BindProperty, Required]
    public UserRole Role { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var client = _httpClientFactory.CreateClient(AppConstants.BusBookingAPI);
        var payload = new
        {
            email = Email,
            fullName = FullName,
            password = Password,
            role = Role.ToString()
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, AppConstants.JsonContentType);

        var response = await client.PostAsync(AppConstants.AuthRegister, content);
        if (response.IsSuccessStatusCode)
        {
            TempData[AppConstants.RegistrationSuccessMessageKey] = AppConstants.RegistrationSuccessMessage;
            return RedirectToPage(AppConstants.Login, new { returnUrl = ReturnUrl });
        }

        // read optional API error
        var apiError = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(apiError)
            ? AppConstants.RegistrationFailureMessage
            : apiError);
        return Page();
    }
}
