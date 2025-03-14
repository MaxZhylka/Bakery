using backend.Core.DTOs;
using backend.Core.Models;
using Microsoft.Data.SqlClient;

namespace backend.Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
        Task<PaginatedResult<OrderDTO>> GetOrdersAsync(PaginationParameters paginationParameters, SqlConnection? connection = null);
        Task<PaginatedResult<OrderDTO>> GetOrdersByUserIdAsync(PaginationParameters paginationParameters, Guid userId);
        Task<OrderEntity> GetOrderAsync(Guid id, SqlConnection? connection = null);
        Task<OrderEntity> CreateOrderAsync(OrderEntity order);
        Task<OrderEntity> UpdateOrderAsync(Guid id, OrderEntity order);
        Task<OrderEntity> DeleteOrderAsync(Guid id);
    }
}