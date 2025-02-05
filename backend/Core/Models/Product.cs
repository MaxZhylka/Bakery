namespace backend.Core.Models
{
  public class ProductEntity
  {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required int ProductCount { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}