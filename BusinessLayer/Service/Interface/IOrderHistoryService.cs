using BOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IOrderHistoryService
    {
        Task<IEnumerable<OrderHistory>> GetAllOrderHistoriesAsync();
        Task<IEnumerable<OrderHistory>> GetOrderHistoriesByUserIdAsync(int userId);
        Task<IEnumerable<OrderHistory>> GetOrderHistoriesByOrderIdAsync(int orderId); // Nếu cần
    }
}
