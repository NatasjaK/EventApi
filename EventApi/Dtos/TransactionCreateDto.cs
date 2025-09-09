using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class TransactionCreateDto
{
    [Required] public int EventId { get; set; }
    [Range(0, 100000000)] public decimal Amount { get; set; }
    [Required] public DateTime Date { get; set; }
}
public class TransactionUpdateDto : TransactionCreateDto { }

public class TransactionReadDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? EventTitle { get; set; }
}
