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
        
        public LoanStatus Status { get; set; }
    
        public DateTime CreatedAt { get; set; }
    }
}
