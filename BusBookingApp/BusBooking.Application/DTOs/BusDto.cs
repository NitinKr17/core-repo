namespace BusBooking.Application.DTOs;

public class BusDto
{
    public int Id { get; set; }
    public string OperatorName { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
}
