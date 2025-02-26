
namespace backend.Core.DTOs
{
  public class CustomerReportDto
  {
    public Guid CustomerId { get; set; }
    public required string CustomerName { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
  }
}
