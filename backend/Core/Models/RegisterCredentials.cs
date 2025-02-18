using System.ComponentModel.DataAnnotations;
using backend.Core.Attributes;

namespace backend.Core.Models
{
  public class RegisterCredentialsEntity
  {
    public required string Name { get; set; }

    [EmailAddress]
    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
  }

}
