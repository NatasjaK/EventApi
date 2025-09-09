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
public class MerchandisesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public MerchandisesController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MerchandiseReadDto>>> GetAll() =>
        Ok(await _db.Merchandises.AsNoTracking()
            .ProjectTo<MerchandiseReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MerchandiseReadDto>> GetById(int id)
    {
        var dto = await _db.Merchandises.AsNoTracking()
            .Where(m => m.Id == id)
            .ProjectTo<MerchandiseReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<MerchandiseReadDto>> Create(MerchandiseCreateDto dto)
    {
        var entity = _mapper.Map<Merchandise>(dto);
        _db.Merchandises.Add(entity);
        await _db.SaveChangesAsync();

        var read = await _db.Merchandises.AsNoTracking()
            .Where(m => m.Id == entity.Id)
            .ProjectTo<MerchandiseReadDto>(_mapper.ConfigurationProvider)
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MerchandiseUpdateDto dto)
    {
        var entity = await _db.Merchandises.FindAsync(id);
        if (entity is null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Merchandises.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Merchandises.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
