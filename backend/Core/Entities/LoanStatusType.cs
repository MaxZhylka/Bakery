using backend.Core.Entities;

namespace backend.Core.Entities;
public class LoanStatusType
{
  public int Id { get; set; }
  public string Name { get; set; } 
  public ICollection<Loan> Loans { get; set; }
}
