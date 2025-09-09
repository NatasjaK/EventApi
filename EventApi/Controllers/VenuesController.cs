using EventApi.Data;
using EventApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public VenuesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venue>>> GetAll() =>
            await _context.Venues.Include(v => v.Events).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Venue>> GetById(int id)
        {
            var entity = await _context.Venues
                .Include(v => v.Events)
                .FirstOrDefaultAsync(v => v.Id == id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Venue>> Create(Venue entity)
        {
            _context.Venues.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Venue entity)
        {
            if (id != entity.Id) return BadRequest();
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Venues.FindAsync(id);
            if (entity is null) return NotFound();
            _context.Venues.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
