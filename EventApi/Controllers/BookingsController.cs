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
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public BookingsController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingReadDto>>> GetAll() =>
        Ok(await _db.Bookings.AsNoTracking()
            .ProjectTo<BookingReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingReadDto>> GetById(int id)
    {
        var dto = await _db.Bookings.AsNoTracking()
            .Where(b => b.Id == id)
            .ProjectTo<BookingReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<BookingReadDto>> Create(BookingCreateDto dto)
    {
        var entity = _mapper.Map<Booking>(dto);
        _db.Bookings.Add(entity);
        await _db.SaveChangesAsync();

        var read = await _db.Bookings.AsNoTracking()
            .Where(b => b.Id == entity.Id)
            .ProjectTo<BookingReadDto>(_mapper.ConfigurationProvider)
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, BookingUpdateDto dto)
    {
        var entity = await _db.Bookings.FindAsync(id);
        if (entity is null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Bookings.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Bookings.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
