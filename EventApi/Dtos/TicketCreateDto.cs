using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class TicketCreateDto
{
    [Required] public int BookingId { get; set; }
    [Required, StringLength(120)] public string QRCode { get; set; } = null!;
    [Required, StringLength(40)] public string Status { get; set; } = "Active";
}
public class TicketUpdateDto : TicketCreateDto { }

public class TicketReadDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public string QRCode { get; set; } = null!;
    public string Status { get; set; } = null!;
}
