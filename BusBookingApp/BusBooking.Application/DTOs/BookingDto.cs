namespace BusBooking.Application.DTOs;

public class BookingDto
{
    public int UserId { get; set; }
    public int BusId { get; set; }
    public List<PassengerDto> Passengers { get; set; } = new();
}
