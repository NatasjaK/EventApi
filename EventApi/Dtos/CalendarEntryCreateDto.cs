using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class CalendarEntryCreateDto
{
    [Required] public int EventId { get; set; }
    [Required, StringLength(120)] public string Title { get; set; } = null!;
    [Required] public DateTime EndDate { get; set; }
}
public class CalendarEntryUpdateDto : CalendarEntryCreateDto { }

public class CalendarEntryReadDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Title { get; set; } = null!;
    public DateTime EndDate { get; set; }
    public string? EventTitle { get; set; }
}
