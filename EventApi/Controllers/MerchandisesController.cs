// AI-generated with assistance
using EventApi.Data;
using EventApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchandisesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MerchandisesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Merchandise>>> GetAll() =>
            await _context.Merchandises.Include(m => m.Package).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Merchandise>> GetById(int id)
        {
            var entity = await _context.Merchandises
                .Include(m => m.Package)
                .FirstOrDefaultAsync(m => m.Id == id);
            return entity is null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<Merchandise>> Create(Merchandise entity)
        {
            _context.Merchandises.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Merchandise entity)
        {
            if (id != entity.Id) return BadRequest();
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Merchandises.FindAsync(id);
            if (entity is null) return NotFound();
            _context.Merchandises.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
