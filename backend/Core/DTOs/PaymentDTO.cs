using System.Text.Json.Serialization;
using backend.Core.Enums;

namespace backend.Core.DTOs
{
    public class PaymentDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public decimal Value { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid LoanId { get; set; }
    }
}
