using System.Text.Json.Serialization;
using backend.Core.Enums;

namespace backend.Core.DTOs
{
  public class UserActionDTO
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required UserRole UserRole { get; set; }
    public required string Email { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Operations Operation { get; set; }
    public required string Details { get; set; }
    public DateTime Timestamp { get; set; }
  }
}
