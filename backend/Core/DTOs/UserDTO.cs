using backend.Core.Enums;

namespace backend.Core.DTOs
{
  public class UserDTO
  {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required UserRole Role { get; set; }
    
    public string? AccessToken { get; set; }
  }

}
