
namespace backend.Core.Models
{
  public class RefreshTokenEntity
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string RefreshToken { get; set; }

    public DateTime Expiration { get; set; }
    public required string DeviceId { get; set; }
  }

}
