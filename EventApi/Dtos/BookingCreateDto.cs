using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class BookingCreateDto
{
    [Required] public int UserId { get; set; }
    [Required] public int EventId { get; set; }
    [Required] public DateTime BookingDate { get; set; }
    [Required, StringLength(40)] public string Status { get; set; } = "Confirmed";
}
public class BookingUpdateDto : BookingCreateDto { }

public class BookingReadDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Status { get; set; } = null!;
    public string? UserName { get; set; }
    public string? EventTitle { get; set; }
    public int TicketCount { get; set; }
}
