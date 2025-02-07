using Microsoft.AspNetCore.Mvc;
using backend.Core.Interfaces;
using backend.Core.DTOs;
using System.Threading.Tasks;
using Core.Attributes;

namespace backend.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController(IOrderService orderService) : ControllerBase
  {
    private readonly IOrderService _orderService = orderService;

    [ErrorHandler]
    [HttpGet]
    public async Task<IEnumerable<OrderDTO>> Get()
    {
      return await _orderService.GetOrders();
    }

    [ErrorHandler]
    [HttpGet("{id}")]
    public async Task<OrderDTO> Get(Guid id)
    {
      return await _orderService.GetOrder(id);
    }

    [ErrorHandler]
    [HttpPost]
    public async Task<OrderDTO> Post([FromBody] OrderDTO order)
    {
      return await _orderService.CreateOrder(order);
    }

    [ErrorHandler]
    [HttpPut("{id}")]
    public async Task<OrderDTO> Put(Guid id, [FromBody] OrderDTO order)
    {
      return await _orderService.UpdateOrder(id, order);
    }

    [ErrorHandler]
    [HttpDelete("{id}")]
    public async Task<OrderDTO> Delete(Guid id)
    {
      return await _orderService.DeleteOrder(id);
    }
  }
}