using BOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync();
        Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetailId);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<int> CreateOrderDetailAsync(OrderDetail orderDetail);
        Task<int> UpdateOrderDetailAsync(OrderDetail orderDetail);
        Task<int> DeleteOrderDetailAsync(int orderDetailId);
    }
}
