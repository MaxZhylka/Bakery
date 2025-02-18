using backend.Core.DTOs;
using backend.Core.Interfaces;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace backend.Core.Services
{
    public class OrderService(IOrderRepository orderRepository, IMapper mapper) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedResult<OrderDTO>> GetOrders(PaginationParameters paginationParameters)
        {
            return await _orderRepository.GetOrdersAsync(paginationParameters);
        }

        public async Task<PaginatedResult<OrderDTO>> GetOrdersByUserId(PaginationParameters paginationParameters, Guid userId) {
            return await _orderRepository.GetOrdersByUserIdAsync(paginationParameters, userId);
        }
        public async Task<OrderDTO> GetOrder(Guid id)
        {
            var order = await _orderRepository.GetOrderAsync(id);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> CreateOrder(OrderDTO order)
        {
            var orderEntity = _mapper.Map<OrderEntity>(order);
            orderEntity.Id = Guid.NewGuid();
            var createdOrder = await _orderRepository.CreateOrderAsync(orderEntity);
            return _mapper.Map<OrderDTO>(createdOrder);
        }

        public async Task<OrderDTO> UpdateOrder(Guid id, OrderDTO order)
        {
            var orderEntity = _mapper.Map<OrderEntity>(order);
            var updatedOrder = await _orderRepository.UpdateOrderAsync(id, orderEntity);
            return _mapper.Map<OrderDTO>(updatedOrder);
        }

        public async Task<OrderDTO> DeleteOrder(Guid id)
        {
            var deletedOrder = await _orderRepository.DeleteOrderAsync(id);
            return _mapper.Map<OrderDTO>(deletedOrder);
        }
    }
}