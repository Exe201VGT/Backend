using BOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IOrderHistoryRepository
    {
        Task<IEnumerable<OrderHistory>> GetAllOrderHistoriesAsync();
        Task<IEnumerable<OrderHistory>> GetOrderHistoriesByUserIdAsync(int userId);
        Task<IEnumerable<OrderHistory>> GetOrderHistoriesByOrderIdAsync(int orderId); 
    }
    public class OrderHistoryRepository : IOrderHistoryRepository
    {
        private readonly VietNongContext _context;

        public OrderHistoryRepository(VietNongContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderHistory>> GetAllOrderHistoriesAsync()
        {
            return await _context.OrderHistories.ToListAsync();
        }

        public async Task<IEnumerable<OrderHistory>> GetOrderHistoriesByUserIdAsync(int userId)
        {
            return await _context.OrderHistories.Where(oh => oh.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<OrderHistory>> GetOrderHistoriesByOrderIdAsync(int orderId)
        {
            return await _context.OrderHistories.Where(oh => oh.OrderId == orderId).ToListAsync();
        }
    }
}
