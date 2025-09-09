// AI-generated with assistance
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventApi.Data;
using EventApi.Models;

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TransactionsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAll() =>
            await _context.Transactions.Include(t => t.Event).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Transaction>> GetById(int id)
        {
            var entity = await _context.Transactions
                .Include(t => t.Event)
                .FirstOrDefaultAsync(t => t.Id == id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> Create(Transaction entity)
        {
            _context.Transactions.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Transaction entity)
        {
            if (id != entity.Id) return BadRequest();
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Transactions.FindAsync(id);
            if (entity is null) return NotFound();
            _context.Transactions.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
