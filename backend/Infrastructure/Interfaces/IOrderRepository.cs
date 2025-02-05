using backend.Core.Models;
using Microsoft.Data.SqlClient;

namespace backend.Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderEntity>> GetOrdersAsync(SqlConnection? connection = null);
        Task<OrderEntity> GetOrderAsync(Guid id, SqlConnection? connection = null);
        Task<OrderEntity> CreateOrderAsync(OrderEntity order);
        Task<OrderEntity> UpdateOrderAsync(Guid id, OrderEntity order);
        Task<OrderEntity> DeleteOrderAsync(Guid id);
    }
}