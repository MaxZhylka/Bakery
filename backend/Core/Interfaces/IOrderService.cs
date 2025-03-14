using backend.Core.DTOs;
using backend.Core.Models;


namespace backend.Core.Interfaces
{
    public interface IOrderService
    {
        Task<PaginatedResult<OrderDTO>> GetOrders(PaginationParameters paginationParameters);
        Task<PaginatedResult<OrderDTO>> GetOrdersByUserId(PaginationParameters paginationParameters, Guid userId);
        Task<OrderDTO> GetOrder(Guid id);
        Task<OrderDTO> CreateOrder(OrderEntity order);
        Task<OrderDTO> UpdateOrder(Guid id, OrderDTO order);
        Task<OrderDTO> DeleteOrder(Guid id);
    }
}
