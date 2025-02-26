namespace backend.Core.DTOs
{
  public class OrderTrendsByCustomerDto
  {
    public string? CustomerName { get; set; }
    public string? OrderMonth { get; set; }
    public int TotalOrders { get; set; }
  }
}