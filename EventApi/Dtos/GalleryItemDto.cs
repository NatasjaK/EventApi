namespace EventApi.Dtos;

public class GalleryItemDto
{
    public int Id { get; set; }
    public int? EventId { get; set; }
    public string Url { get; set; } = default!;
    public string? Caption { get; set; }
}
