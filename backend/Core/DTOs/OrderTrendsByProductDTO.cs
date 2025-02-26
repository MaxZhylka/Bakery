namespace backend.Core.DTOs
{
  public class OrderTrendsByProductDto
  {
    public string? ProductName { get; set; }
    public string? OrderMonth { get; set; }
    public int TotalSold { get; set; }
  }
}