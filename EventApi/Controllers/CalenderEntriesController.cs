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
public class CalendarEntriesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public CalendarEntriesController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEntryReadDto>>> GetAll() =>
        Ok(await _db.CalendarEntries.AsNoTracking()
            .ProjectTo<CalendarEntryReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CalendarEntryReadDto>> GetById(int id)
    {
        var dto = await _db.CalendarEntries.AsNoTracking()
            .Where(c => c.Id == id)
            .ProjectTo<CalendarEntryReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<CalendarEntryReadDto>> Create(CalendarEntryCreateDto dto)
    {
        var entity = _mapper.Map<CalendarEntry>(dto);
        _db.CalendarEntries.Add(entity);
        await _db.SaveChangesAsync();

        var read = await _db.CalendarEntries.AsNoTracking()
            .Where(c => c.Id == entity.Id)
            .ProjectTo<CalendarEntryReadDto>(_mapper.ConfigurationProvider)
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CalendarEntryUpdateDto dto)
    {
        var entity = await _db.CalendarEntries.FindAsync(id);
        if (entity is null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.CalendarEntries.FindAsync(id);
        if (entity is null) return NotFound();
        _db.CalendarEntries.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
