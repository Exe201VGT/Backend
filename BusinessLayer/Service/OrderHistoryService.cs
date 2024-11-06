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
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly IOrderHistoryRepository _orderHistoryRepository;

        public OrderHistoryService(IOrderHistoryRepository orderHistoryRepository)
        {
            _orderHistoryRepository = orderHistoryRepository;
        }

        public async Task<IEnumerable<OrderHistory>> GetAllOrderHistoriesAsync()
        {
            return await _orderHistoryRepository.GetAllOrderHistoriesAsync();
        }

        public async Task<IEnumerable<OrderHistory>> GetOrderHistoriesByUserIdAsync(int userId)
        {
            return await _orderHistoryRepository.GetOrderHistoriesByUserIdAsync(userId);
        }

        public async Task<IEnumerable<OrderHistory>> GetOrderHistoriesByOrderIdAsync(int orderId)
        {
            return await _orderHistoryRepository.GetOrderHistoriesByOrderIdAsync(orderId);
        }
    }
}
