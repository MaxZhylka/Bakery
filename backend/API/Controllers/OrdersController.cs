using Microsoft.AspNetCore.Mvc;
using backend.Core.Interfaces;
using backend.Core.DTOs;
using Core.Attributes;
using Microsoft.AspNetCore.Authorization;
using backend.Core.Models;

namespace backend.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController(IOrderService orderService) : ControllerBase
  {
    private readonly IOrderService _orderService = orderService;

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager,User")]
    [HttpGet]
    public async Task<PaginatedResult<OrderDTO>> GetOrders([FromQuery] PaginationParameters paginationParameters)
    {
      return await _orderService.GetOrders(paginationParameters);
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager,User")]
    [HttpGet("ByUserId/{userId}")]
    public async Task<PaginatedResult<OrderDTO>> GetOrdersByUserId([FromQuery] PaginationParameters paginationParameters, Guid userId)
    {
      return await _orderService.GetOrdersByUserId(paginationParameters, userId);
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager,User")]
    [HttpGet("{id}")]
    public async Task<OrderDTO> GetOrder(Guid id)
    {
      return await _orderService.GetOrder(id);
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager,User")]
    [HttpPost]
    public async Task<OrderDTO> CreateOrder([FromBody] OrderEntity order)
    {
      return await _orderService.CreateOrder(order);
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager,User")]
    [HttpPut("{id}")]
    public async Task<OrderDTO> UpdateOrder(Guid id, [FromBody] OrderDTO order)
    {
      return await _orderService.UpdateOrder(id, order);
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager,User")]
    [HttpDelete("{id}")]
    public async Task<OrderDTO> DeleteOrder(Guid id)
    {
      return await _orderService.DeleteOrder(id);
    }
  }
}