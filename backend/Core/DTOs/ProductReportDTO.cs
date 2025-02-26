
namespace backend.Core.DTOs
{
  public class ProductReportDto
  {
    public Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    public int TotalSold { get; set; }
    public decimal TotalRevenue { get; set; }
  }
}