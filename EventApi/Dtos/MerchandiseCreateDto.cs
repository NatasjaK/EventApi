using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class MerchandiseCreateDto
{
    [Required] public int PackageId { get; set; }
    [Required, StringLength(120)] public string Title { get; set; } = null!;
    [Range(0, 1000000)] public decimal Price { get; set; }
}
public class MerchandiseUpdateDto : MerchandiseCreateDto { }

public class MerchandiseReadDto
{
    public int Id { get; set; }
    public int PackageId { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
}
