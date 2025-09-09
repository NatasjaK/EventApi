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
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public TicketsController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketReadDto>>> GetAll() =>
        Ok(await _db.Tickets.AsNoTracking()
            .ProjectTo<TicketReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketReadDto>> GetById(int id)
    {
        var dto = await _db.Tickets.AsNoTracking()
            .Where(t => t.Id == id)
            .ProjectTo<TicketReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<TicketReadDto>> Create(TicketCreateDto dto)
    {
        var entity = _mapper.Map<Ticket>(dto);
        _db.Tickets.Add(entity);
        await _db.SaveChangesAsync();

        var read = await _db.Tickets.AsNoTracking()
            .Where(t => t.Id == entity.Id)
            .ProjectTo<TicketReadDto>(_mapper.ConfigurationProvider)
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TicketUpdateDto dto)
    {
        var entity = await _db.Tickets.FindAsync(id);
        if (entity is null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Tickets.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Tickets.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
