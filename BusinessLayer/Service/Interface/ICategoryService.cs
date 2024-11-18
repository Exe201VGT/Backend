using BOs.Models;
using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<int> CreateCategoryAsync(Category category);
        Task<int> UpdateCategoryAsync(Category category);
        Task<int> DeleteCategoryAsync(int id);
    }
}
