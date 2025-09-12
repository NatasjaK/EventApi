using EventApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventApi.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, ILogger? log = null)
    {
        await db.Database.MigrateAsync();

        log?.LogInformation("EventApi seeding started");

        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new User { Name = "Alicia Svensson", Email = "alicia@example.com", PasswordHash = "x" },
                new User { Name = "Jonas Berg", Email = "jonas@example.com", PasswordHash = "x" }
            );
            await db.SaveChangesAsync();
            log?.LogInformation("Seeded Users");
        }

        var u1 = await db.Users.OrderBy(u => u.Id).FirstAsync();

        if (!await db.Venues.AnyAsync())
        {
            db.Venues.AddRange(
                new Venue { Name = "Nordic Arena", MapImage = "map-nordic.png", Address = "Vallhallavägen 1, Stockholm" },
                new Venue { Name = "City Hall", MapImage = "map-cityhall.png", Address = "Stadsgatan 2, Göteborg" }
            );
            await db.SaveChangesAsync();
            log?.LogInformation("Seeded Venues");
        }

        var v1 = await db.Venues.OrderBy(v => v.Id).FirstAsync();

        if (!await db.Events.AnyAsync())
        {
            db.Events.AddRange(
                new Event
                {
                    Title = "Echo Beats Festival",
                    Description = "Electronic music & lights",
                    Date = DateTime.UtcNow.AddDays(14),
                    Location = "Stockholm",
                    VenueId = v1.Id,
                    MaxSeats = 300,
                    Price = 80m
                },
                new Event
                {
                    Title = "Culinary Delights Festival",
                    Description = "Street food & chefs on stage",
                    Date = DateTime.UtcNow.AddDays(20),
                    Location = "Göteborg",
                    VenueId = v1.Id,
                    MaxSeats = 250,
                    Price = 75m
                }
            );
            await db.SaveChangesAsync();
            log?.LogInformation("Seeded Events");
        }

        var e1 = await db.Events.OrderBy(e => e.Id).FirstOrDefaultAsync();
        if (e1 is null)
        {
            log?.LogWarning("No events exist after seeding – aborting the rest.");
            return; 
        }

        if (!await db.Bookings.AnyAsync())
        {
            var b1 = new Booking
            {
                EventId = e1.Id,
                UserId = u1.Id,
                BookingDate = DateTime.UtcNow.AddDays(-2),
                Status = "Confirmed"
            };
            db.Bookings.Add(b1);
            await db.SaveChangesAsync();

            db.Tickets.AddRange(
                new Ticket { BookingId = b1.Id, EventId = e1.Id, SeatNumber = "A-12", Price = e1.Price, Status = "Issued" },
                new Ticket { BookingId = b1.Id, EventId = e1.Id, SeatNumber = "A-13", Price = e1.Price, Status = "Issued" }
            );
            await db.SaveChangesAsync();
            log?.LogInformation("Seeded Bookings & Tickets");
        }

        if (!await db.Feedbacks.AnyAsync())
        {
            db.Feedbacks.AddRange(
                new Feedback { EventId = e1.Id, UserId = u1.Id, Rating = 4, Comment = "Great event!", CreatedAt = DateTime.UtcNow.AddDays(-1) },
                new Feedback { EventId = e1.Id, Rating = 5, Comment = "Nice vibe.", CreatedAt = DateTime.UtcNow.AddHours(-10) }
            );
            await db.SaveChangesAsync();
            log?.LogInformation("Seeded Feedbacks");
        }

        if (!await db.Packages.AnyAsync())
        {
            db.Packages.AddRange(
                new Package { EventId = e1.Id, Title = "General Admission", Price = e1.Price },
                new Package { EventId = e1.Id, Title = "VIP Lounge", Price = e1.Price + 250 }
            );
            await db.SaveChangesAsync();
            log?.LogInformation("Seeded Packages");
        }

        log?.LogInformation("EventApi seeding finished");
    }
}
