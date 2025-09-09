// AI-generated helper
namespace EventApi.Dtos.Dashboard;

public class UpcomingEventDto
{
    public int EventId { get; set; }
    public string Title { get; set; } = null!;
    public DateTime Date { get; set; }
    public string? VenueName { get; set; }
    public int MaxSeats { get; set; }
    public int TicketsSold { get; set; }
    public int SpotsLeft => Math.Max(0, MaxSeats - TicketsSold);
}
