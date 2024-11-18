using AutoMapper;
using BOs.Models;
using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace VietNongAPI2.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class ProductController : ODataController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }


        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "Product not found" });
            }
            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = _mapper.Map<Product>(productCreateDTO);
            var result = await _productService.CreateProductAsync(product);
            if (result > 0)
            {
                return Ok(new { Message = "Product created successfully" });
            }
            return BadRequest(new { Message = "Failed to create product" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDTO productUpdateDTO)
        {
            if (id != productUpdateDTO.ProductId)
            {
                return BadRequest(new { Message = "Invalid Product ID" });
            }
            var product = _mapper.Map<Product>(productUpdateDTO);
            var result = await _productService.UpdateProductAsync(product);
            if (result > 0)
            {
                return Ok(new { Message = "Product updated successfully" });
            }
            return BadRequest(new { Message = "Failed to update product" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result > 0)
            {
                return Ok(new { Message = "Product deleted successfully" });
            }
            return BadRequest(new { Message = "Failed to delete product" });
        }
    }
}
