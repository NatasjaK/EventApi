using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class EventCreateDto
{
    [Required, StringLength(120)]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required, StringLength(100)] public string Location { get; set; } = null!;
    [Range(1, 100000)] public int MaxSeats { get; set; }
    [Range(0, 1000000)] public decimal Price { get; set; }
    [Required] public int VenueId { get; set; }
}

public class EventUpdateDto : EventCreateDto { }

public class EventReadDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; } = null!;
    public int MaxSeats { get; set; }
    public decimal Price { get; set; }
    public int VenueId { get; set; }
    public string? VenueName { get; set; }
}
