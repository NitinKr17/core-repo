namespace BusBooking.Shared.Constants;

public static class AppConstants
{
    // API
    public const string BusBookingAPI = "BusBookingAPI";

    // Session Keys
    public const string JwtTokenSessionKey = "JwtToken";
    public const string JwtClaim = "jwt";
    public const string UserIdSessionKey = "UserId";

    // API Endpoints
    public const string BookingCreateEndpoint = "booking/create";
    public const string BookingHistoryEndpoint = "booking/user/{0}";
    public const string BusSearchEndpoint = "bus/search";
    public const string BusListEndpoint = "bus/all";
    public const string BusCreateEndpoint = "bus/create";
    public const string AuthLogin = "auth/login";
    public const string AuthRegister = "auth/register";
    public const string BookingsByUser = "booking/user/{0}";
    public const string BookingById = "booking/{0}";
    public const string BookingCancel = "booking/{0}/cancel";
    public const string BookingAddPassenger = "booking/{0}/passengers";
    public const string BookingRemovePassenger = "booking/{0}/passengers/{1}";
    public const string BookingsAllEndpoint = "booking/all";
    public const string BookingsByBusEndpoint = "booking/bus/{0}";

    // Pages
    public const string Root = "/";
    public const string Home = "Home/Index";
    public const string AdminBuses = "/Admin/Buses";
    public const string Login = "/Login";
    public const string Logout = "/Logout";
    public const string AccessDenied = "/AccessDenied";
    public const string Error = "/Error";
    public const string BookingHistory = "/BookingHistory";

    // Roles
    public const string AdminRole = "Admin";
    public const string UserRole = "User";

    // Misc
    public const string DateFormat = "yyyy-MM-dd";
    public const string SuccessMessageKey = "SuccessMessage";
    public const string RegistrationSuccessMessageKey = "RegistrationSuccessMessage";
    public const string ErrorMessageKey = "Error";
    public const string ErrorMessage = "ErrorMessage";
    public const string JwtToken = "JwtToken";
    public const string UserId = "userId";
    public const string UserIdClaimType = "uid";
    public const string APIBaseUrlKey = "ApiSettings:BaseUrl";

    // Error Messages
    public const string NoPassengersError = "Please add at least one passenger.";
    public const string InvalidPassengerDataError = "Invalid passenger data.";
    public const string InvalidBookingError = "Invalid booking.";
    public const string FailedPassengerError = "Failed to add passenger";
    public const string FutureBookingError = "Only future bookings can be changed.";
    public const string BookingFailedError = "Failed to create booking.";
    public const string PassengerFailedError = "Failed to remove passenger.";
    public const string InvalidCredentials = "Invalid credentials";
    public const string AvailableSeatsError = "Not enough available seats on this bus.";
    public const string BusNotFound = "Bus with ID {0} not found.";
    public const string UserClaimError = "Unable to identify the user from token.";
    public const string ErrorOccurred = "An error occurred while fetching bus data.";
    public const string CouldNotLoadError = "Could not load data.";
    public const string PasswordMatchError = "Passwords do not match.";
    public const string FailedCancelError = "Failed to cancel booking.";
    public const string FailedLoadBookingsError = "Failed to load bookings.";

    // Validation Errors
    public const string NameRequiredError = "Name is required.";
    public const string NameCharatersError = "Passenger name cannot exceed 100 characters.";
    public const string AgeRequiredError = "Age is required.";
    public const string InvalidAgeError = "Invalid age.";
    public const string GenderRequiredError = "Gender is required.";
    public const string GenderTypeError = "Passenger gender must be Male or Female.";
    public const string OperatorRequiredError = "Operator is required.";
    public const string OperatorCharatersError = "Operator name cannot exceed 100 characters.";
    public const string FromRequiredError = "From is required.";
    public const string FromLengthError = "From must be <= 100 chars.";
    public const string ToRequiredError = "To is required.";
    public const string ToLengthError = "To must be <= 100 chars.";
    public const string DepartureRequiredError = "Departure is required.";
    public const string ArrivalRequiredError = "Arrival is required.";
    public const string TotalSeatsRequiredError = "Total seats is required.";
    public const string SeatsInvalidError = "Invalid seats.";
    public const string AvailableSeatsRequiredError = "Available seats is required.";
    public const string AvailableSeatsGreaterError = "Exceeds total seats.";


    // UI Messages
    public const string PassengerRemoved = "Passenger removed.";
    public const string PassengerAdded = "Passenger added.";
    public const string BookingSuccessMessage = "Booking confirmed successfully!";
    public const string BookingCancelledMessage = "Booking cancelled.";
    public const string RegistrationSuccessMessage = "Account created. Please log in.";
    public const string RegistrationFailureMessage = "Registration failed.";
    public const string BusCreatedMessage = "Bus created successfully.";
    public const string BusCreateFailedError = "Failed to create bus.";

    public const int DefaultUserId = 1;

    // Content Types
    public const string JsonContentType = "application/json";
}
