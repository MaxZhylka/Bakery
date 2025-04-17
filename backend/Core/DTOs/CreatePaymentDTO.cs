using System.Text.Json.Serialization;
using backend.Core.Enums;

namespace backend.Core.DTOs
{
    public class CreatePaymentDTO
    {
        public Guid UserId { get; set; }

        public decimal Value { get; set; }

        public Guid LoanId { get; set; }
    }
}
