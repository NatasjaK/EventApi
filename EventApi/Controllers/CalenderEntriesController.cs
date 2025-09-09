// AI-generated with assistance
using EventApi.Data;
using EventApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEntriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CalendarEntriesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarEntry>>> GetAll() =>
            await _context.CalendarEntries.Include(c => c.Event).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CalendarEntry>> GetById(int id)
        {
            var entity = await _context.CalendarEntries
                .Include(c => c.Event)
                .FirstOrDefaultAsync(c => c.Id == id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<CalendarEntry>> Create(CalendarEntry entity)
        {
            _context.CalendarEntries.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CalendarEntry entity)
        {
            if (id != entity.Id) return BadRequest();
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.CalendarEntries.FindAsync(id);
            if (entity is null) return NotFound();
            _context.CalendarEntries.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
