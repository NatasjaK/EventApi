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
public class InvoicesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public InvoicesController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceReadDto>>> GetAll() =>
        Ok(await _db.Invoices.AsNoTracking()
            .ProjectTo<InvoiceReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InvoiceReadDto>> GetById(int id)
    {
        var dto = await _db.Invoices.AsNoTracking()
            .Where(i => i.Id == id)
            .ProjectTo<InvoiceReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceReadDto>> Create(InvoiceCreateDto dto)
    {
        var entity = _mapper.Map<Invoice>(dto);
        _db.Invoices.Add(entity);
        await _db.SaveChangesAsync();

        var read = await _db.Invoices.AsNoTracking()
            .Where(i => i.Id == entity.Id)
            .ProjectTo<InvoiceReadDto>(_mapper.ConfigurationProvider)
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, InvoiceUpdateDto dto)
    {
        var entity = await _db.Invoices.FindAsync(id);
        if (entity is null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Invoices.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Invoices.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
