


using System.Text.Json.Serialization;
using backend.Core.Enums;

namespace backend.Core.Models
{

  public class LoggerPaginationParameters : PaginationParameters
  {

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole? UserRole { get; set; } = null;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Operations? Operation { get; set; } = null;

    public string? SearchQuery { get; set; } = null;

  }
}