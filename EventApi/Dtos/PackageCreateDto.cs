using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class PackageCreateDto
{
    [Required] public int EventId { get; set; }
    [Required, StringLength(120)] public string Title { get; set; } = null!;
    [Range(0, 1000000)] public decimal Price { get; set; }
}
public class PackageUpdateDto : PackageCreateDto { }

public class PackageReadDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public int MerchandiseCount { get; set; }
}
