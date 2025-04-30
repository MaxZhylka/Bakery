
using System.ComponentModel.DataAnnotations.Schema;
using backend.Core.Enums;

namespace backend.Core.Entities
{
  public class Payment
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public Guid LoanId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }

    public int PaymentStatusTypeId { get; set; }
    public PaymentStatusType PaymentStatusType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public User User { get; set; } = null!;
  }

}
