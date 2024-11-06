using BOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync();
        Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetailId);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<int> CreateOrderDetailAsync(OrderDetail orderDetail);
        Task<int> UpdateOrderDetailAsync(OrderDetail orderDetail);
        Task<int> DeleteOrderDetailAsync(int orderDetailId);
    }

    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly VietNongContext _context;

        public OrderDetailRepository(VietNongContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync()
        {
            return await _context.OrderDetails.ToListAsync();
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetailId)
        {
            return await _context.OrderDetails.FindAsync(orderDetailId);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();
        }

        public async Task<int> CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteOrderDetailAsync(int orderDetailId)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(orderDetailId);
            if (orderDetail == null) return 0;

            _context.OrderDetails.Remove(orderDetail);
            return await _context.SaveChangesAsync();
        }
    }
}
