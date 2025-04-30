
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
    public int LoanApplicationStatusTypeId { get; set; }
    public LoanTerm Term { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? RejectionReasonId { get; set; }
    public RejectionReason? RejectionReason { get; set; }
    public User User { get; set; } = null!;
    public LoanApplicationStatusType LoanApplicationStatusType { get; set; } = null!;
  }

}
