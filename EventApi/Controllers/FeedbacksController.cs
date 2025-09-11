// AI-generated with assistance
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EventApi.Data;
using EventApi.Dtos;          
using EventApi.Models;       
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FeedbacksController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public FeedbacksController(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    // GET /api/feedbacks?eventId=1&top=50
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeedbackReadDto>>> GetAll(
        [FromQuery] int? eventId,
        [FromQuery] int top = 50)
    {
        top = Math.Clamp(top, 1, 200);

        var q = _db.Feedbacks.AsNoTracking();
        if (eventId.HasValue)
            q = q.Where(f => f.EventId == eventId.Value);

        var list = await q
            .OrderByDescending(f => f.CreatedAt)
            .Take(top)
            .ProjectTo<FeedbackReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return Ok(list);
    }

    // GET /api/feedbacks/123
    [HttpGet("{id:int}")]
    public async Task<ActionResult<FeedbackReadDto>> GetById(int id)
    {
        var dto = await _db.Feedbacks.AsNoTracking()
            .Where(f => f.Id == id)
            .ProjectTo<FeedbackReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return dto is null ? NotFound() : Ok(dto);
    }

    // POST /api/feedbacks
    [HttpPost]
    public async Task<ActionResult<FeedbackReadDto>> Create([FromBody] FeedbackCreateDto dto)
    {
        if (dto.Rating < 1 || dto.Rating > 5)
            return BadRequest("Rating must be between 1 and 5.");

        // CreatedAt 
        var entity = _mapper.Map<Feedback>(dto);
        _db.Feedbacks.Add(entity);
        await _db.SaveChangesAsync();

        var read = await _db.Feedbacks.AsNoTracking()
            .Where(f => f.Id == entity.Id)
            .ProjectTo<FeedbackReadDto>(_mapper.ConfigurationProvider)
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    // PUT /api/feedbacks/123 
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] FeedbackUpdateDto dto)
    {
        var entity = await _db.Feedbacks.FindAsync(id);
        if (entity is null) return NotFound();

        if (dto.Rating.HasValue && (dto.Rating.Value < 1 || dto.Rating.Value > 5))
            return BadRequest("Rating must be between 1 and 5.");

        _mapper.Map(dto, entity); 
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/feedbacks/123
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Feedbacks.FindAsync(id);
        if (entity is null) return NotFound();

        _db.Feedbacks.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
