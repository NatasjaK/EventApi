using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class VenueCreateDto
{
    [Required, StringLength(100)]
    public string Name { get; set; } = null!;
    public string? MapImage { get; set; }
    [Required, StringLength(200)]
    public string Address { get; set; } = null!;
}

public class VenueUpdateDto : VenueCreateDto { }

public class VenueReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? MapImage { get; set; }
    public string Address { get; set; } = null!;
    public int EventCount { get; set; }
}
