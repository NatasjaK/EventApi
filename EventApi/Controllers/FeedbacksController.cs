using EventApi.Data;
using EventApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly AppDbContext _context;
        public FeedbacksController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAll() =>
            await _context.Feedbacks.Include(f => f.Event).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Feedback>> GetById(int id)
        {
            var entity = await _context.Feedbacks
                .Include(f => f.Event)
                .FirstOrDefaultAsync(f => f.Id == id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Feedback>> Create(Feedback entity)
        {
            _context.Feedbacks.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Feedback entity)
        {
            if (id != entity.Id) return BadRequest();
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Feedbacks.FindAsync(id);
            if (entity is null) return NotFound();
            _context.Feedbacks.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
