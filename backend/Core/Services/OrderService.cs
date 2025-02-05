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

        public IEnumerable<OrderDTO> GetOrders()
        {
            var orders = _orderRepository.GetOrdersAsync().Result;
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public OrderDTO GetOrder(Guid id)
        {
            var order = _orderRepository.GetOrderAsync(id).Result;
            return _mapper.Map<OrderDTO>(order);
        }

        public OrderDTO CreateOrder(OrderDTO order)
        {   
            var orderEntity = _mapper.Map<OrderEntity>(order);
            orderEntity.Id = Guid.NewGuid();
            var createdOrder = _orderRepository.CreateOrderAsync(orderEntity).Result;
            return _mapper.Map<OrderDTO>(createdOrder);
        }

        public OrderDTO UpdateOrder(Guid id, OrderDTO order)
        {
            var orderEntity = _mapper.Map<OrderEntity>(order);
            var updatedOrder = _orderRepository.UpdateOrderAsync(id, orderEntity).Result;
            return _mapper.Map<OrderDTO>(updatedOrder);
        }

        public OrderDTO DeleteOrder(Guid id)
        {
            var deletedOrder = _orderRepository.DeleteOrderAsync(id).Result;
            return _mapper.Map<OrderDTO>(deletedOrder);
        }
    }
}