using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class MessageCreateDto
{
    [Required] public int UserId { get; set; }
    [Required, StringLength(200)] public string Title { get; set; } = null!;
    [Required] public string Body { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
public class MessageUpdateDto : MessageCreateDto { }

public class MessageReadDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime SentAt { get; set; }
    public string? UserName { get; set; }
}
