using System.ComponentModel.DataAnnotations;
using backend.Core.Attributes;
using backend.Core.Enums;

namespace backend.Core.DTOs
{
  public class UserCreateDTO
  {
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
  }

}
