using System.Text.Json.Serialization;
using backend.Core.Enums;

namespace backend.Core.DTOs
{
    public class LoanDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public int Percent { get; set; }

        public decimal ValueToPayOnCurrentMonth { get; set; }
        public decimal ValueToPay { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LoanStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public required DateTime NextPaymentDate { get; set; }
        public required decimal CompletedValue { get; set; }
        public required decimal LeftValue { get; set; }

        public required string ClientEmail { get; set; }
    }
}
