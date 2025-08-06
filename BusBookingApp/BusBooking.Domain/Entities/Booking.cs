namespace BusBooking.Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BusId { get; set; }
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    public int NumberOfSeats { get; set; }
    public ICollection<Passenger>? Passengers { get; set; }
    public User? User { get; set; }
    public Bus? Bus { get; set; }
}
