
namespace backend.Core.Entities;
public class LoanApplicationStatusType
{
  public int Id { get; set; }
  public string Name { get; set; } 
  public ICollection<LoanApplication> LoanApplications { get; set; }
}
