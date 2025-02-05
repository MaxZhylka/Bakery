using backend.Core.DTOs;


namespace backend.Core.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderDTO> GetOrders();
        OrderDTO GetOrder(Guid id);
        OrderDTO CreateOrder(OrderDTO order);
        OrderDTO UpdateOrder(Guid id, OrderDTO order);
        OrderDTO DeleteOrder(Guid id);
    }
}
