namespace BusBooking.Shared.Constants;

public class APIConstants
{    
    // Jwt Keys
    public const string JwtSection = "Jwt";
    public const string JwtKey = "Key";
    public const string JwtIssuer = "Issuer";
    public const string JwtAudience = "Audience";
    public const string UserIdClaimType = "uid";

    public const string InvalidLoginMessage = "Invalid credentials";
    public const string InvalidLoginLogTemplate = "Invalid login attempt for email: {0}";

    // Controller Keys
    public const string Controller = "[controller]";

    // Misc
    public const string AlreadyExistError = "Email already exists.";
    public const string DBConnectionKey = "BusBookingDB";
    public const string AllowFrontend = "AllowFrontend";
    public const string Bearer = "Bearer";
    public const string JWT = "JWT";
    public const string V1 = "v1";
    public const string Authorization = "Authorization";
    public const string APITitle = "BusBooking API";
    public const string APIEndpoint = "https://localhost:7041";

    // Error Messages
    public const string BookingNotFound = "Booking not found.";
    public const string BusNotFound = "Bus with ID {0} not found.";
    public const string PastJourneyModifyError = "Cannot modify past journey.";
    public const string AvailableSeatsError = "Not enough available seats on this bus.";
    public const string PassengerNotFound = "Passenger not found";
    public const string BearerMessage = "Enter 'Bearer' [space] and your JWT token.";
}
