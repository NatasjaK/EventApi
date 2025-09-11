// AI-generated seed helper
using EventApi.Data;
using EventApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (await db.Venues.AnyAsync()) return;

        // --- Venues ---
        var echo = new Venue { Name = "Echo Arena", MapImage = "echo-arena-map.png", Address = "1200 S Flower St, Los Angeles, CA" };
        var nord = new Venue { Name = "Nordic Arena", MapImage = "nordic-arena-map.png", Address = "Vallhallavägen 1, Stockholm" };

        // --- Events ---
        var e1 = new Event
        {
            Title = "Echo Beats Festival",
            Description = "Rhythm & beats all night",
            Date = new DateTime(2025, 05, 15, 18, 00, 00, DateTimeKind.Utc),
            Location = "Los Angeles",
            MaxSeats = 5000,
            Price = 79m,
            Venue = echo,
            Packages = new List<Package>
            {
                new Package { Title = "General Admission", Price = 79m },
                new Package { Title = "VIP Lounge Package", Price = 199m,
                    Merchandises = new List<Merchandise>
                    {
                        new Merchandise { Title = "Eco T-Shirt", Price = 25m },
                        new Merchandise { Title = "VIP Lanyard", Price = 9m }
                    }
                }
            },
            CalendarEntries = new List<CalendarEntry>
            {
                new CalendarEntry { Title = "Doors Open", EndDate = new DateTime(2025,05,15,18,00,00, DateTimeKind.Utc) },
                new CalendarEntry { Title = "Main Act",   EndDate = new DateTime(2025,05,15,21,00,00, DateTimeKind.Utc) }
            }
        };

        var e2 = new Event
        {
            Title = "Culinary Delights Festival",
            Description = "Street food & demos",
            Date = new DateTime(2025, 06, 01, 10, 00, 00, DateTimeKind.Utc),
            Location = "Stockholm",
            MaxSeats = 2000,
            Price = 59m,
            Venue = nord,
            Packages = new List<Package>
            {
                new Package { Title = "General Admission", Price = 59m }
            }
        };

        // --- Users ---
        var u1 = new User { Name = "Alicia Svensson", Email = "alicia@example.com", PasswordHash = "dev-only" };
        var u2 = new User { Name = "Jonas Berg", Email = "jonas@example.com", PasswordHash = "dev-only" };

        // --- Bookings + Tickets ---
        var b1 = new Booking
        {
            User = u1,
            Event = e1,
            BookingDate = DateTime.UtcNow.AddDays(-14),
            Status = "Confirmed",
            Tickets = new List<Ticket>
            {
                new Ticket { QRCode = "ECHO-001-A", Status = "Active" },
                new Ticket { QRCode = "ECHO-001-B", Status = "Active" }
            }
        };
        var b2 = new Booking
        {
            User = u2,
            Event = e2,
            BookingDate = DateTime.UtcNow.AddDays(-10),
            Status = "Pending",
            Tickets = new List<Ticket>
            {
                new Ticket { QRCode = "CULI-002-A", Status = "Active" }
            }
        };

        // --- Invoices ---
        var i1 = new Invoice { User = u1, Event = e1, Amount = 158m, InvoiceNumber = "INV-1001" };
        var i2 = new Invoice { User = u2, Event = e2, Amount = 59m, InvoiceNumber = "INV-1002" };

        // --- Feedbacks ---
        var f1 = new Feedback { Event = e1, Rating = 5, Comment = "Magiskt ljud och ljus!" };
        var f2 = new Feedback { Event = e2, Rating = 4, Comment = "Grym mat, lite kö." };

        // --- Messages ---
        var m1 = new Message { User = u1, Title = "Välkommen!", Body = "Tack för din bokning.", IsRead = false, SentAt = DateTime.UtcNow.AddDays(-14) };
        var m2 = new Message { User = u2, Title = "Betalningspåminnelse", Body = "Din faktura INV-1002 är obetald.", IsRead = false, SentAt = DateTime.UtcNow.AddDays(-9) };

        // --- Transactions  EventApi ---
        var t1 = new Transaction { Event = e1, Amount = 158m, Date = DateTime.UtcNow.AddDays(-14) };
        var t2 = new Transaction { Event = e2, Amount = 59m, Date = DateTime.UtcNow.AddDays(-10) };

        db.AddRange(echo, nord, e1, e2, u1, u2, b1, b2, i1, i2, f1, f2, m1, m2, t1, t2);
        await db.SaveChangesAsync();
    }
}
