using AutoMapper;
using BOs.Models;
using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;
using BusinessLayer.Service.Interface;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateCategoryAsync(Category category)
        {
            return await _categoryRepository.CreateAsync(category);
        }

        public async Task<int> UpdateCategoryAsync(Category category)
        {
            return await _categoryRepository.UpdateAsync(category);
        }

        public async Task<int> DeleteCategoryAsync(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }
    }
}
