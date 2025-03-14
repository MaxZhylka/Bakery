namespace backend.Core.DTOs
{
  public class OrderDTO
  {
    public Guid Id { get; set; }
    public required Guid ProductId { get; set; }

    public string? ProductName { get; set; }
    public required decimal Price { get; set; }
    public required int ProductCount { get; set; }
    public DateTime? CreatedAt { get; set; }
    public required string CustomerName { get; set; }
  }

}
