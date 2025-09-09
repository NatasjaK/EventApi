using System.ComponentModel.DataAnnotations;

namespace EventApi.Dtos;

public class UserCreateDto
{
    [Required, StringLength(100)] public string Name { get; set; } = null!;
    [Required, EmailAddress, StringLength(200)] public string Email { get; set; } = null!;
    [Required] public string PasswordHash { get; set; } = null!;
}

public class UserUpdateDto : UserCreateDto { }

public class UserReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int BookingCount { get; set; }
    public int InvoiceCount { get; set; }
}
