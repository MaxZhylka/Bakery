using backend.Core.Enums;

namespace backend.Core.Entities
{
  public class LoanApplication
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public int Value { get; set; }

    public LoanApplicationStatus Status { get; set; }

    public LoanTerm Term { get; set; }
    public DateTime CreatedAt { get; set; }
  }

}
