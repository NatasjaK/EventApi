using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class InvoiceCreateDto
{
    [Required] public int UserId { get; set; }
    [Required] public int EventId { get; set; }
    [Range(0, 1000000)] public decimal Amount { get; set; }
    [Required, StringLength(40)] public string InvoiceNumber { get; set; } = null!;
}
public class InvoiceUpdateDto : InvoiceCreateDto { }

public class InvoiceReadDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public decimal Amount { get; set; }
    public string InvoiceNumber { get; set; } = null!;
    public string? UserEmail { get; set; }
    public string? EventTitle { get; set; }
}
