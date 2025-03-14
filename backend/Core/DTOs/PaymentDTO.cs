using backend.Core.Enums;

namespace backend.Core.DTOs
{
    public class PaymentDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public int Value { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
