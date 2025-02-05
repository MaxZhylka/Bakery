namespace backend.Core.DTOs
{
  public class ProductDTO
  {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required int ProductCount { get; set; }
    public DateTime? CreatedAt { get; set; }
  }

}
