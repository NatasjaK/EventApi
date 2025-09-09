using EventApi.Data;
using EventApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public EventsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAll() =>
            await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Packages)
                .Include(e => e.CalendarEntries)
                .ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Event>> GetById(int id)
        {
            var entity = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Packages)
                .Include(e => e.CalendarEntries)
                .FirstOrDefaultAsync(e => e.Id == id);

            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Event>> Create(Event entity)
        {
            _context.Events.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Event entity)
        {
            if (id != entity.Id) return BadRequest();
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Events.FindAsync(id);
            if (entity is null) return NotFound();
            _context.Events.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
