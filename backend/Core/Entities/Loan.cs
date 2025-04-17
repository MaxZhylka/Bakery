
using System.ComponentModel.DataAnnotations.Schema;
using backend.Core.Enums;

namespace backend.Core.Entities
{
  public class Loan
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public int Percent { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValueToPayOnCurrentMonth { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal ValueToPay { get; set; }
    public DateTime NextPaymentDate { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public required decimal CompletedValue { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public required decimal LeftValue { get; set; }
    public LoanStatus Status { get; set; }

    public LoanTerm Term { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
  }

}
