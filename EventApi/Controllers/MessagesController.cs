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
public class MessagesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public MessagesController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageReadDto>>> GetAll() =>
        Ok(await _db.Messages.AsNoTracking()
            .ProjectTo<MessageReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MessageReadDto>> GetById(int id)
    {
        var dto = await _db.Messages.AsNoTracking()
            .Where(m => m.Id == id)
            .ProjectTo<MessageReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<MessageReadDto>> Create(MessageCreateDto dto)
    {
        var entity = _mapper.Map<Message>(dto);
        _db.Messages.Add(entity);
        await _db.SaveChangesAsync();

        var read = await _db.Messages.AsNoTracking()
            .Where(m => m.Id == entity.Id)
            .ProjectTo<MessageReadDto>(_mapper.ConfigurationProvider)
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MessageUpdateDto dto)
    {
        var entity = await _db.Messages.FindAsync(id);
        if (entity is null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Messages.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Messages.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
