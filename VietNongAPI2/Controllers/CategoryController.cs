using AutoMapper;
using BOs.Models;
using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;
using BusinessLayer.Service.Interface;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace VietNongAPI2.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class CategoryController : ODataController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;


        public CategoryController(ICategoryService categoryService, IMapper mapper, Cloudinary cloudinary)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _cloudinary = cloudinary;

        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return Ok(categoryDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = "Category not found" });
            }
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateDTO categoryCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string CategoryImageUrl = null;
            if (categoryCreateDTO.Image != null)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(categoryCreateDTO.Image.FileName, categoryCreateDTO.Image.OpenReadStream()),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    CategoryImageUrl = uploadResult.SecureUrl.ToString(); // Lưu URL ảnh
                }
                else
                {
                    return StatusCode((int)uploadResult.StatusCode, new { Message = uploadResult.Error.Message });
                }
            }
            var category = _mapper.Map<Category>(categoryCreateDTO);
            category.Image = CategoryImageUrl;
            var result = await _categoryService.CreateCategoryAsync(category);
            if (result > 0)
            {
                return Ok(new { Message = "Category created successfully" });
            }
            return BadRequest(new { Message = "Failed to create category" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDTO categoryUpdateDTO)
        {
            if (id != categoryUpdateDTO.CategoryId)
            {
                return BadRequest(new { Message = "Invalid Category ID" });
            }
            var category = _mapper.Map<Category>(categoryUpdateDTO);
            var result = await _categoryService.UpdateCategoryAsync(category);
            if (result > 0)
            {
                return Ok(new { Message = "Category updated successfully" });
            }
            return BadRequest(new { Message = "Failed to update category" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (result > 0)
            {
                return Ok(new { Message = "Category deleted successfully" });
            }
            return BadRequest(new { Message = "Failed to delete category" });
        }
    }
}
