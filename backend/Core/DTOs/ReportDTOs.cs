namespace backend.Core.DTOs
{

  public class LoanApplicationReportDto
  {
    public int Year { get; set; }
    public int Month { get; set; }
    public int Total { get; set; }
    public int Rejected { get; set; }
    public double RejectedPercent { get; set; }
  }

  public class MoneyFlowDto
  {
    public decimal TotalIssued { get; set; }
    public decimal TotalReturned { get; set; }
  }

  public class UserApplicationsDto
  {
    public string Email { get; set; } = string.Empty;
    public int ApplicationsCount { get; set; }
  }

  public class MonthlyPaymentsDto
  {
    public int Year { get; set; }
    public int Month { get; set; }
    public int PaymentsCount { get; set; }
    public decimal AverageAmount { get; set; }
  }

}