
namespace backend.Core.Models
{
  public class OrderEntity
  {
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }

    public required int ProductCount { get; set; }
    public required decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string CustomerName { get; set; }
  }

}
