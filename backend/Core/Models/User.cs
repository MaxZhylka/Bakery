using System.ComponentModel.DataAnnotations;
using backend.Core.Attributes;
using backend.Core.Enums;
public class UserEntity
{
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [EmailAddress]
    [Required]
    public required string Email { get; set; } 

    [Password]
    [Required]
    public required string Password { get; set; }

    [Required]
    public required UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; }

}
