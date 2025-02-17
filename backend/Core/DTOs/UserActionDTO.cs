using backend.Core.Enums;

namespace backend.Core.DTOs
{
  public class UserActionDTO
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public Operations Operation { get; set; }
    public required string Details { get; set; }
    public DateTime Timestamp { get; set; }
  }
}
