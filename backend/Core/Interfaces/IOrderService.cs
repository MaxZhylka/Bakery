using backend.Core.DTOs;


namespace backend.Core.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrders();
        Task<OrderDTO> GetOrder(Guid id);
        Task<OrderDTO> CreateOrder(OrderDTO order);
        Task<OrderDTO> UpdateOrder(Guid id, OrderDTO order);
        Task<OrderDTO> DeleteOrder(Guid id);
    }
}
