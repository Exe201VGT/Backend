using BOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ISellerService
    {
        Task<IEnumerable<Seller>> GetAllSellersAsync();
        Task<Seller> GetSellerByIdAsync(int sellerId);
        Task<Seller> GetSellerByUserIdAsync(int userId); // Lấy seller theo UserId
        Task<Seller> RegisterSellerAsync(Seller seller); // Đăng ký Seller
        Task<bool> UpdateSellerAsync(Seller seller);
        Task<bool> DeleteSellerAsync(int sellerId);
    }
}
