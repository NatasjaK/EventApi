using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentsApi.Data;
using PaymentsApi.Models;
using PaymentsApi.Models.Dtos;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly PaymentsDb _db;
    public TransactionsController(PaymentsDb db) { _db = db; }

    [HttpGet]
    public async Task<IEnumerable<Transaction>> Get() =>
        await _db.Transactions.AsNoTracking().ToListAsync();

    [HttpGet("total")]
    public async Task<decimal> GetTotal([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var start = (from ?? DateTime.UtcNow.AddDays(-30)).Date;
        var end = (to ?? DateTime.UtcNow).Date.AddDays(1);
        return await _db.Transactions
            .Where(t => t.Date >= start && t.Date < end)
            .SumAsync(t => (decimal?)t.Amount) ?? 0m;
    }
    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<RevenuePointDto>>> GetRange(
    [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var end = (to ?? DateTime.UtcNow).Date.AddDays(1);
        var start = (from ?? DateTime.UtcNow.AddDays(-30)).Date;

        var grouped = await _db.Transactions.AsNoTracking()
            .Where(t => t.Date >= start && t.Date < end)
            .GroupBy(t => t.Date.Date)
            .Select(g => new { Date = g.Key, Amount = g.Sum(x => x.Amount) })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return Ok(grouped.Select(x => new RevenuePointDto
        {
            Label = x.Date.ToString("yyyy-MM-dd"),
            Amount = x.Amount
        }));

    }
}
