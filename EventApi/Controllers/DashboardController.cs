// AI-generated with assistance
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventApi.Data;
using EventApi.Dtos.Dashboard;

namespace EventApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;
    public DashboardController(AppDbContext db) { _db = db; }

    // GET: /api/dashboard/summary
    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var now = DateTime.UtcNow;

        var totalEvents = await _db.Events.CountAsync();
        var upcomingEvents = await _db.Events.CountAsync(e => e.Date >= now);
        var totalUsers = await _db.Users.CountAsync();
        var totalBookings = await _db.Bookings.CountAsync();
        var ticketsSold = await _db.Tickets.CountAsync();
        var totalRevenue = await _db.Transactions.SumAsync(t => (decimal?)t.Amount) ?? 0m;
        var avgRating = Math.Round(await _db.Feedbacks.AverageAsync(f => (double?)f.Rating) ?? 0.0, 2);
        var unreadMessages = await _db.Messages.CountAsync(m => !m.IsRead);

        return Ok(new DashboardSummaryDto
        {
            TotalEvents = totalEvents,
            UpcomingEvents = upcomingEvents,
            TotalUsers = totalUsers,
            TotalBookings = totalBookings,
            TicketsSold = ticketsSold,
            TotalRevenue = totalRevenue,
            AvgRating = avgRating,
            UnreadMessages = unreadMessages
        });
    }

    // GET: /api/dashboard/recent-bookings?top=5
    [HttpGet("recent-bookings")]
    public async Task<ActionResult<IEnumerable<RecentBookingDto>>> GetRecentBookings([FromQuery] int top = 5)
    {
        top = Math.Clamp(top, 1, 50);

        var items = await _db.Bookings
            .AsNoTracking()
            .OrderByDescending(b => b.BookingDate)
            .Take(top)
            .Select(b => new RecentBookingDto
            {
                Id = b.Id,
                UserName = b.User != null ? b.User.Name : "",
                EventTitle = b.Event != null ? b.Event.Title : "",
                BookingDate = b.BookingDate,
                Tickets = b.Tickets.Count,
                Status = b.Status
            })
            .ToListAsync();

        return Ok(items);
    }

    // GET: /api/dashboard/revenue-range?from=2025-05-01&to=2025-05-31
    [HttpGet("revenue-range")]
    public async Task<ActionResult<IEnumerable<RevenuePointDto>>> GetRevenueRange(
    [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var end = (to ?? DateTime.UtcNow).Date.AddDays(1);
        var start = (from ?? DateTime.UtcNow.AddDays(-30)).Date;

        var grouped = await _db.Transactions
            .AsNoTracking()
            .Where(t => t.Date >= start && t.Date < end)
            .GroupBy(t => t.Date.Date)               
            .Select(g => new { Date = g.Key, Amount = g.Sum(x => x.Amount) })
            .OrderBy(x => x.Date)
            .ToListAsync();

        var data = grouped
            .Select(x => new RevenuePointDto
            {
                Label = x.Date.ToString("yyyy-MM-dd"), 
                Amount = x.Amount
            })
            .ToList();

        return Ok(data);
    }

    // GET: /api/dashboard/top-events?limit=5
    [HttpGet("top-events")]
    public async Task<ActionResult<IEnumerable<EventPerformanceDto>>> GetTopEvents([FromQuery] int limit = 5)
    {
        limit = Math.Clamp(limit, 1, 20);

        var items = await _db.Events
            .AsNoTracking()
            .Select(e => new EventPerformanceDto
            {
                EventId = e.Id,
                Title = e.Title,
                TicketsSold = e.Bookings.SelectMany(b => b.Tickets).Count(),
                Revenue = e.Transactions.Sum(t => (decimal?)t.Amount) ?? 0m,
                AvgRating = e.Feedbacks.Average(f => (double?)f.Rating) ?? 0.0
            })
            .OrderByDescending(x => x.Revenue)
            .ThenByDescending(x => x.TicketsSold)
            .Take(limit)
            .ToListAsync();

        return Ok(items);
    }

    // GET: /api/dashboard/upcoming?days=30
    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<UpcomingEventDto>>> GetUpcoming([FromQuery] int days = 30)
    {
        days = Math.Clamp(days, 1, 365);
        var now = DateTime.UtcNow;
        var end = now.AddDays(days);

        var items = await _db.Events
            .AsNoTracking()
            .Where(e => e.Date >= now && e.Date <= end)
            .Select(e => new UpcomingEventDto
            {
                EventId = e.Id,
                Title = e.Title,
                Date = e.Date,
                VenueName = e.Venue != null ? e.Venue.Name : null,
                MaxSeats = e.MaxSeats,
                TicketsSold = e.Bookings.SelectMany(b => b.Tickets).Count()
            })
            .OrderBy(e => e.Date)
            .ToListAsync();

        return Ok(items);
    }
}
