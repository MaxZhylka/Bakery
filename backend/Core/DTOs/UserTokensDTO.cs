using backend.Core.Enums;

namespace backend.Core.DTOs
{
  public class UserTokensDTO
  {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required UserRole Role { get; set; }

    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }

    public required string DeviceId { get; set; }
  }

}
