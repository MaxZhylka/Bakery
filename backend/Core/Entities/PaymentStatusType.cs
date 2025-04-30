using backend.Core.Entities;

namespace backend.Core.Entities;
public class PaymentStatusType
{
  public int Id { get; set; }
  public string Name { get; set; } 
  public ICollection<Payment> Payments { get; set; }
}
