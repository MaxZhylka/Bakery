using Microsoft.AspNetCore.Mvc;
using backend.Core.Interfaces;
using backend.Core.DTOs;
using Core.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace backend.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController(IOrderService orderService) : ControllerBase
  {
    private readonly IOrderService _orderService = orderService;

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager")]
    [HttpGet]
    public async Task<IEnumerable<OrderDTO>> GetOrders()
    {
      return await _orderService.GetOrders();
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager")]
    [HttpGet("{id}")]
    public async Task<OrderDTO> GetOrder(Guid id)
    {
      return await _orderService.GetOrder(id);
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<OrderDTO> CreateOrder([FromBody] OrderDTO order)
    {
      return await _orderService.CreateOrder(order);
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager")]
    [HttpPut("{id}")]
    public async Task<OrderDTO> UpdateOrder(Guid id, [FromBody] OrderDTO order)
    {
      return await _orderService.UpdateOrder(id, order);
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager")]
    [HttpDelete("{id}")]
    public async Task<OrderDTO> DeleteOrder(Guid id)
    {
      return await _orderService.DeleteOrder(id);
    }
  }
}