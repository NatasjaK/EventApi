// AI-generated seed helper for PaymentsApi
using Microsoft.EntityFrameworkCore;
using PaymentsApi.Data;
using PaymentsApi.Models;

public static class PaymentsSeeder
{
    public static async Task SeedAsync(PaymentsDb db)
    {
        await db.Database.MigrateAsync();
        if (await db.Transactions.AnyAsync()) return;

        var baseDate = DateTime.UtcNow.Date;

        var rows = new List<Transaction>
        {
            new() { EventId = 1, Amount = 79m,  Date = baseDate.AddDays(-14).AddHours(12) },
            new() { EventId = 1, Amount = 79m,  Date = baseDate.AddDays(-14).AddHours(12) },
            new() { EventId = 2, Amount = 59m,  Date = baseDate.AddDays(-10).AddHours( 9) },
            new() { EventId = 1, Amount = 199m, Date = baseDate.AddDays(-7 ).AddHours(15) }, 
            new() { EventId = 2, Amount = 59m,  Date = baseDate.AddDays(-5 ).AddHours(10) },
            new() { EventId = 1, Amount = 79m,  Date = baseDate.AddDays(-3 ).AddHours(18) },
            new() { EventId = 2, Amount = 59m,  Date = baseDate.AddDays(-2 ).AddHours(11) }
        };

        db.AddRange(rows);
        await db.SaveChangesAsync();
    }
}
