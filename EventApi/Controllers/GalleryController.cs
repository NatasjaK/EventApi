using EventApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EventApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GalleryController : ControllerBase
{
    private static readonly List<GalleryItemDto> _items = new()
    {
        new GalleryItemDto { Id = 1, EventId = 1, Url = "/images/gallery/echo-1.jpg",     Caption = "Echo Beats crowd" },
        new GalleryItemDto { Id = 2, EventId = 1, Url = "/images/gallery/echo-2.jpg",     Caption = "Main stage" },
        new GalleryItemDto { Id = 3, EventId = 2, Url = "/images/gallery/culinary-1.jpg", Caption = "Culinary booth" },
        new GalleryItemDto { Id = 4, EventId = null, Url = "/images/gallery/venue-1.jpg", Caption = "Venue overview" }
    };

    [HttpGet]
    public ActionResult<IEnumerable<GalleryItemDto>> Get([FromQuery] int? eventId)
        => Ok(eventId.HasValue ? _items.Where(i => i.EventId == eventId) : _items);

    [HttpGet("{id:int}")]
    public ActionResult<GalleryItemDto> GetById(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public ActionResult<GalleryItemDto> Create([FromBody] GalleryItemDto dto)
    {
        var nextId = _items.Count == 0 ? 1 : _items.Max(i => i.Id) + 1;
        var item = new GalleryItemDto
        {
            Id = nextId,
            EventId = dto.EventId,
            Url = dto.Url,
            Caption = dto.Caption
        };
        _items.Add(item);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item is null) return NotFound();
        _items.Remove(item);
        return NoContent();
    }
}
