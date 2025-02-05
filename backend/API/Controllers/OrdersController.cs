using Microsoft.AspNetCore.Mvc;
using backend.Core.Interfaces;
using backend.Core.DTOs;

namespace backend.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController(IOrderService orderService) : ControllerBase
  {
    private readonly IOrderService _orderService = orderService;

        [HttpGet]
    public IEnumerable<OrderDTO> Get()
    {
      return _orderService.GetOrders();
    }

    [HttpGet("{id}")]
    public OrderDTO Get(Guid id)
    {
      return  _orderService.GetOrder(id);
    }

    [HttpPost]
    public OrderDTO Post([FromBody] OrderDTO order)
    {
      return _orderService.CreateOrder(order);
    }

    [HttpPut("{id}")]
    public OrderDTO Put(Guid id, [FromBody] OrderDTO order)
    {
      return _orderService.UpdateOrder(id, order);
    }

    [HttpDelete("{id}")]
    public OrderDTO Delete(Guid id)
    {
      return _orderService.DeleteOrder(id);
    }
  }
}