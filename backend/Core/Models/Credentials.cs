using System.ComponentModel.DataAnnotations;
using System.Globalization;
using backend.Core.Attributes;

namespace backend.Core.Models
{public class CredentialsEntity
{
    [EmailAddress]
    [Required]
    public required string Email { get; set; }

    [Password]
    [Required]
    public required string Password { get; set; }
}

}
