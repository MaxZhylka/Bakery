using System.Text.Json.Serialization;
using backend.Core.Enums;

namespace backend.Core.DTOs
{
    public class LoanApplicationDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public decimal Value { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LoanApplicationStatus Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LoanTerm Term { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? RejectionReason { get; set; }

        public required string ClientEmail { get; set; }
    }
}
