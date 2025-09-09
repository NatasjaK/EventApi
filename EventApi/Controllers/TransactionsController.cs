using AutoMapper;
using AutoMapper.QueryableExtensions;
using EventApi.Data;
using EventApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventApi.Models;

namespace EventApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    public TransactionsController(AppDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionReadDto>>> GetAll() =>
        Ok(await _db.Transactions.AsNoTracking()
            .ProjectTo<TransactionReadDto>(_mapper.ConfigurationProvider)
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TransactionReadDto>> GetById(int id)
    {
        var dto = await _db.Transactions.AsNoTracking()
            .Where(t => t.Id == id)
            .ProjectTo<TransactionReadDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<TransactionReadDto>> Create(TransactionCreateDto dto)
    {
        var entity = _mapper.Map<Transaction>(dto);
        _db.Transactions.Add(entity);
        await _db.SaveChangesAsync();

        var read = await _db.Transactions.AsNoTracking()
            .Where(t => t.Id == entity.Id)
            .ProjectTo<TransactionReadDto>(_mapper.ConfigurationProvider)
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TransactionUpdateDto dto)
    {
        var entity = await _db.Transactions.FindAsync(id);
        if (entity is null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.Transactions.FindAsync(id);
        if (entity is null) return NotFound();
        _db.Transactions.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
