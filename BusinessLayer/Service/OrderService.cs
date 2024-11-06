using BOs.Models;
using BusinessLayer.Service.Interface;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(int sellerId)
        {
            return await _orderRepository.GetOrdersBySellerIdAsync(sellerId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(int buyerId) 
        {
            return await _orderRepository.GetOrdersByBuyerIdAsync(buyerId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _orderRepository.GetOrdersByStatusAsync(status);
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            return await _orderRepository.CreateOrderAsync(order);
        }

        public async Task<int> UpdateOrderAsync(Order order)
        {
            return await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task<int> DeleteOrderAsync(int orderId)
        {
            return await _orderRepository.DeleteOrderAsync(orderId);
        }
    }
}
