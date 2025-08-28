namespace BusBooking.Application.DTOs;

public class BookingHistoryDto
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public int NumberOfSeats { get; set; }
    public BusDto Bus { get; set; }
    public List<PassengerDto> Passengers { get; set; } = new();
}
