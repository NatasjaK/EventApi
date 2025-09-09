// AI-generated helper
namespace EventApi.Dtos.Dashboard;

public class RecentBookingDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string EventTitle { get; set; } = null!;
    public DateTime BookingDate { get; set; }
    public int Tickets { get; set; }
    public string Status { get; set; } = null!;
}
