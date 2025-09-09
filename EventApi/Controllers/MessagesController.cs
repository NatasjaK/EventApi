// AI-generated with assistance
using EventApi.Data;
using EventApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MessagesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetAll() =>
            await _context.Messages.Include(m => m.User).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Message>> GetById(int id)
        {
            var entity = await _context.Messages
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Message>> Create(Message entity)
        {
            _context.Messages.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Message entity)
        {
            if (id != entity.Id) return BadRequest();
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Messages.FindAsync(id);
            if (entity is null) return NotFound();
            _context.Messages.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
