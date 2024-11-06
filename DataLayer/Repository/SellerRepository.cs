using BOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ISellerRepository
    {
        Task<IEnumerable<Seller>> GetAllSellersAsync();
        Task<Seller> GetSellerByIdAsync(int sellerId);
        Task<Seller> GetSellerByUserIdAsync(int userId); // Lấy seller theo UserId
        Task<Seller> RegisterSellerAsync(Seller seller); // Đăng ký Seller
        Task<bool> UpdateSellerAsync(Seller seller);
        Task<bool> DeleteSellerAsync(int sellerId);
    }
    public class SellerRepository : ISellerRepository
    {
        private readonly VietNongContext _context;

        public SellerRepository(VietNongContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Seller>> GetAllSellersAsync()
        {
            return await _context.Sellers.ToListAsync();
        }

        public async Task<Seller> GetSellerByIdAsync(int sellerId)
        {
            return await _context.Sellers.Include(s => s.User)
                                         .FirstOrDefaultAsync(s => s.SellerId == sellerId);
        }

        public async Task<Seller> GetSellerByUserIdAsync(int userId)
        {
            return await _context.Sellers.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<Seller> RegisterSellerAsync(Seller seller)
        {
            await _context.Sellers.AddAsync(seller);
            await _context.SaveChangesAsync();
            return seller;
        }

        public async Task<bool> UpdateSellerAsync(Seller seller)
        {
            _context.Sellers.Update(seller);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSellerAsync(int sellerId)
        {
            var seller = await _context.Sellers.FindAsync(sellerId);
            if (seller == null) return false;

            _context.Sellers.Remove(seller);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
