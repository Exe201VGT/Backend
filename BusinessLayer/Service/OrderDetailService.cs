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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync()
        {
            return await _orderDetailRepository.GetAllOrderDetailsAsync();
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetailId)
        {
            return await _orderDetailRepository.GetOrderDetailByIdAsync(orderDetailId);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
        }

        public async Task<int> CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            return await _orderDetailRepository.CreateOrderDetailAsync(orderDetail);
        }

        public async Task<int> UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            return await _orderDetailRepository.UpdateOrderDetailAsync(orderDetail);
        }

        public async Task<int> DeleteOrderDetailAsync(int orderDetailId)
        {
            return await _orderDetailRepository.DeleteOrderDetailAsync(orderDetailId);
        }
    }
}
