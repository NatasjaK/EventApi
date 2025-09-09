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
public class VenuesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public VenuesController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VenueReadDto>>> GetAll() =>
        Ok(await _db.Venues.AsNoTracking()
            .ProjectTo<VenueReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VenueReadDto>> GetById(int id)
    {
        var dto = await _db.Venues.AsNoTracking()
            .Where(v => v.Id == id)
            .ProjectTo<VenueReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<VenueReadDto>> Create(VenueCreateDto dto)
    {
        var entity = _mapper.Map<Venue>(dto);
        _db.Venues.Add(entity);
        await _db.SaveChangesAsync();

        var read = await _db.Venues.AsNoTracking()
            .Where(v => v.Id == entity.Id)
            .ProjectTo<VenueReadDto>(_mapper.ConfigurationProvider)
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, VenueUpdateDto dto)
    {
        var entity = await _db.Venues.FindAsync(id);
        if (entity is null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Venues.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Venues.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
