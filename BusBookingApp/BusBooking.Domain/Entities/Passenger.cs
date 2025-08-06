namespace BusBooking.Domain.Entities;

public class Passenger
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int BookingId { get; set; }
    public Booking? Booking { get; set; }
}
