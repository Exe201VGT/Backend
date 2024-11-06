using BOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IOrderService
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
}
