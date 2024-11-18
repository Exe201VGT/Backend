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
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(Product product);
        Task<int> UpdateProductAsync(Product product);
        Task<int> DeleteProductAsync(int id);
    }
}
