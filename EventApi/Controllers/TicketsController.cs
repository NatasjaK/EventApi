// AI-generated with assistance
using EventApi.Data;
using EventApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TicketsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetAll() =>
            await _context.Tickets.Include(t => t.Booking).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Ticket>> GetById(int id)
        {
            var entity = await _context.Tickets
                .Include(t => t.Booking)
                .FirstOrDefaultAsync(t => t.Id == id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> Create(Ticket entity)
        {
            _context.Tickets.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Ticket entity)
        {
            if (id != entity.Id) return BadRequest();
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Tickets.FindAsync(id);
            if (entity is null) return NotFound();
            _context.Tickets.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
