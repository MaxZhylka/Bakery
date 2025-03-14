
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Core.DTOs
{
  public class CustomerReportDto
  {
    public Guid CustomerId { get; set; }
    public required string CustomerName { get; set; }
    public int TotalOrders { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalSpent { get; set; }
  }
}
