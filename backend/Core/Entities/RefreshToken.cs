
namespace backend.Core.Entities
{
  public class RefreshTokens
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string RefreshToken { get; set; }

    public DateTime Expiration { get; set; }
    public required string DeviceId { get; set; }
    public User User { get; set; } = null!;
  }

}
