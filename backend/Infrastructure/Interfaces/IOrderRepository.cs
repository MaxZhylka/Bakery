using backend.Core.Models;

namespace backend.Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderEntity>> GetOrdersAsync();
        Task<OrderEntity> GetOrderAsync(Guid id);
        Task<OrderEntity> CreateOrderAsync(OrderEntity order);
        Task<OrderEntity> UpdateOrderAsync(Guid id, OrderEntity order);
        Task<OrderEntity> DeleteOrderAsync(Guid id);
    }
}