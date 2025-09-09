using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class FeedbackCreateDto
{
    [Required] public int EventId { get; set; }
    [Range(1, 5)] public int Rating { get; set; }
    [StringLength(1000)] public string? Comment { get; set; }
}
public class FeedbackUpdateDto : FeedbackCreateDto { }

public class FeedbackReadDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string? EventTitle { get; set; }
}
