namespace backend.Core.DTOs
{
  public class OrderReportDto
  {
    public Guid OrderId { get; set; }
    public string? CustomerName { get; set; }
    public string? ProductName { get; set; }
    public int ProductCount { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
  }

}