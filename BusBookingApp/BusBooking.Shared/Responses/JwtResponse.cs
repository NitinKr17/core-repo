using System.Text.Json.Serialization;

namespace BusBooking.Shared.Responses;

public class JwtResponse
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }
}
