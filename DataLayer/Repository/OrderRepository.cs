using BOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(int sellerId);
        Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(int buyerId); 
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        Task<int> CreateOrderAsync(Order order);
        Task<int> UpdateOrderAsync(Order order);
        Task<int> DeleteOrderAsync(int orderId);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly VietNongContext _context;

        public OrderRepository(VietNongContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(int sellerId)
        {
            return await _context.Orders.Where(o => o.SellerId == sellerId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(int buyerId) 
        {
            return await _context.Orders.Where(o => o.BuyerId == buyerId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _context.Orders.Where(o => o.OrderStatus == status).ToListAsync();
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return 0;

            _context.Orders.Remove(order);
            return await _context.SaveChangesAsync();
        }
    }
}
