using BOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetPagedProductsAsync(int page, int pageSize);
        Task<int> GetTotalProductsCountAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<Product> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<IEnumerable<Product>> GetProductsBySellerIdAsync(int sellerId, int page, int pageSize);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly VietNongContext _context;

        public ProductRepository(VietNongContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetPagedProductsAsync(int page, int pageSize)
        {
            return await _context.Products
                .OrderBy(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<int> GetTotalProductsCountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Seller) 
                .Include(p => p.Reviews) 
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<Product> CreateProductAsync(Product product)

        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product; // Trả về đối tượng Product sau khi lưu
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<Product>> GetProductsBySellerIdAsync(int sellerId, int page, int pageSize)
        {
            return await _context.Products
                .Where(p => p.SellerId == sellerId)
                .OrderBy(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Category)
                .ToListAsync();
        }

    }

}
