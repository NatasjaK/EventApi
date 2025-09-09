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
public class PackagesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public PackagesController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PackageReadDto>>> GetAll() =>
        Ok(await _db.Packages.AsNoTracking()
            .ProjectTo<PackageReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PackageReadDto>> GetById(int id)
    {
        var dto = await _db.Packages.AsNoTracking()
            .Where(p => p.Id == id)
            .ProjectTo<PackageReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<PackageReadDto>> Create(PackageCreateDto dto)
    {
        var entity = _mapper.Map<Package>(dto);
        _db.Packages.Add(entity);
        await _db.SaveChangesAsync();

        var read = await _db.Packages.AsNoTracking()
            .Where(p => p.Id == entity.Id)
            .ProjectTo<PackageReadDto>(_mapper.ConfigurationProvider)
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PackageUpdateDto dto)
    {
        var entity = await _db.Packages.FindAsync(id);
        if (entity is null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Packages.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Packages.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
