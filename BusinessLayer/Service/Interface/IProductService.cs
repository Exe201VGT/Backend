using BOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetPagedProductsAsync(int page, int pageSize);
        Task<int> GetTotalProductsCountAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<Product> CreateProductAsync(Product product, int userId);
        Task<bool> UpdateProductAsync(Product product, int userId);
        Task<bool> DeleteProductAsync(int productId, int userId);
    }
}
