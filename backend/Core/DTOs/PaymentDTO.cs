using System.Text.Json.Serialization;
using backend.Core.Enums;

namespace backend.Core.DTOs
{
    public class PaymentDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public int Value { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid LoanId { get; set; }
    }
}
