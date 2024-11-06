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
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<int> CreateAsync(Product product);
        Task<int> UpdateAsync(Product product);
        Task<int> DeleteAsync(int id);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly VietNongContext _context;

        public ProductRepository(VietNongContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<int> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return 0;

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }
    }
}
