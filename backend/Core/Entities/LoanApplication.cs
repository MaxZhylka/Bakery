
using System.ComponentModel.DataAnnotations.Schema;
using backend.Core.Enums;

namespace backend.Core.Entities
{
  public class LoanApplication
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }

    public LoanApplicationStatus Status { get; set; }
    public LoanTerm Term { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public string? RejectionReason { get; set; }
  }

}
