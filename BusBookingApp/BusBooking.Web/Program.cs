using BusBooking.Shared.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = AppConstants.Login;
    options.LogoutPath = AppConstants.Logout;
    options.AccessDeniedPath = AppConstants.AccessDenied;
});

builder.Services.AddHttpClient(AppConstants.BusBookingAPI, client =>
{
    var apiBaseUrl = builder.Configuration[AppConstants.APIBaseUrlKey];
    client.BaseAddress = new Uri(apiBaseUrl!);
})
#if DEBUG
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return handler;
});
#endif

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(AppConstants.Error);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

app.MapGet(AppConstants.Root, context =>
{
    context.Response.Redirect(AppConstants.Home);
    return Task.CompletedTask;
});

app.Run();
