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
    public FeedbacksController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeedbackReadDto>>> GetAll() =>
        Ok(await _db.Feedbacks.AsNoTracking()
            .ProjectTo<FeedbackReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FeedbackReadDto>> GetById(int id)
    {
        var dto = await _db.Feedbacks.AsNoTracking()
            .Where(f => f.Id == id)
            .ProjectTo<FeedbackReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<FeedbackReadDto>> Create(FeedbackCreateDto dto)
    {
        var entity = _mapper.Map<Feedback>(dto);
        _db.Feedbacks.Add(entity);
        await _db.SaveChangesAsync();

        var read = await _db.Feedbacks.AsNoTracking()
            .Where(f => f.Id == entity.Id)
            .ProjectTo<FeedbackReadDto>(_mapper.ConfigurationProvider)
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, FeedbackUpdateDto dto)
    {
        var entity = await _db.Feedbacks.FindAsync(id);
        if (entity is null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

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
