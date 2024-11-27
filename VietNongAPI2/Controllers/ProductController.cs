using AutoMapper;
using BOs.Models;
using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;
using BusinessLayer.Service;
using BusinessLayer.Service.Interface;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Security.Claims;

namespace VietNongAPI2.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class ProductController : ODataController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ISellerService _sellerService;
        private readonly IUserService _userService;
        private readonly Cloudinary _cloudinary;

        public ProductController(IProductService productService, IMapper mapper, ISellerService sellerService, IUserService userService, Cloudinary cloudinary)
        {
            _productService = productService;
            _mapper = mapper;
            _sellerService = sellerService;
            _userService = userService;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _productService.GetPagedProductsAsync(page, pageSize);
            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(new
            {
                Data = productDTOs,
                Page = page,
                PageSize = pageSize,
                TotalCount = await _productService.GetTotalProductsCountAsync()
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var productDTO = _mapper.Map<ProductDTO>(product);

            // Nếu cần có thông tin seller, bạn có thể thêm vào DTO này
            if (product.Seller != null)
            {
                productDTO.Seller = _mapper.Map<SellerDTO>(product.Seller);
            }

            // Nếu cần có thông tin Reviews, bạn có thể thêm vào DTO này
            var reviews = product.Reviews.Select(r => _mapper.Map<ReviewDTO>(r)).ToList();
            productDTO.Reviews = reviews;

            return Ok(productDTO);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDTO productCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = _userService.GetUserIdFromToken();

            // Upload ảnh lên Cloudinary nếu có
            string productImageUrl = null;
            if (productCreateDto.ProductImage != null)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(productCreateDto.ProductImage.FileName, productCreateDto.ProductImage.OpenReadStream()),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    productImageUrl = uploadResult.SecureUrl.ToString(); // Lưu URL ảnh
                }
                else
                {
                    return StatusCode((int)uploadResult.StatusCode, new { Message = uploadResult.Error.Message });
                }
            }

            // Tạo đối tượng Product
            var product = _mapper.Map<Product>(productCreateDto);
            product.SellerId = (await _sellerService.GetSellerByUserIdAsync(userId)).SellerId;
            product.ProductImage = productImageUrl;

            var createdProduct = await _productService.CreateProductAsync(product, userId);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductId }, _mapper.Map<ProductDTO>(createdProduct));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDTO productUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = _userService.GetUserIdFromToken();

            // Kiểm tra quyền sở hữu sản phẩm
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "Product not found" });
            }
            // Lấy SellerId của người dùng hiện tại
            var seller = await _sellerService.GetSellerByUserIdAsync(userId);
            if (seller == null)
            {
                return Unauthorized(new { Message = "You are not authorized to perform this action" });
            }

            // Upload ảnh mới lên Cloudinary nếu có
            if (productUpdateDto.ProductImage != null)
            {
                if (!string.IsNullOrEmpty(product.ProductImage))
                {
                    var publicId = product.ProductImage.Split('/').Last().Split('.').First();
                    await _cloudinary.DestroyAsync(new DeletionParams(publicId));
                }

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(productUpdateDto.ProductImage.FileName, productUpdateDto.ProductImage.OpenReadStream()),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    product.ProductImage = uploadResult.SecureUrl.ToString();
                }
                else
                {
                    return StatusCode((int)uploadResult.StatusCode, new { Message = uploadResult.Error.Message });
                }
            }

            // Cập nhật thông tin khác
            product.Name = productUpdateDto.Name ?? product.Name;
            product.CategoryId = productUpdateDto.CategoryId ?? product.CategoryId;
            product.Price = productUpdateDto.Price ?? product.Price;
            product.Weight = productUpdateDto.Weight ?? product.Weight;
            product.Description = productUpdateDto.Description ?? product.Description;
            product.StockQuantity = productUpdateDto.StockQuantity ?? product.StockQuantity;

            var success = await _productService.UpdateProductAsync(product,userId);
            if (!success)
            {
                return BadRequest(new { Message = "Failed to update product" });
            }

            return Ok(new { Message = "Product updated successfully", ProductImageUrl = product.ProductImage });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id )
        {
            var userId = _userService.GetUserIdFromToken();

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "Product not found" });
            }
            // Lấy SellerId của người dùng hiện tại
            var seller = await _sellerService.GetSellerByUserIdAsync(userId);
            if (seller == null)
            {
                return Unauthorized(new { Message = "You are not authorized to perform this action" });
            }

            // Xóa ảnh trên Cloudinary nếu có
            if (!string.IsNullOrEmpty(product.ProductImage))
            {
                var publicId = product.ProductImage.Split('/').Last().Split('.').First();
                await _cloudinary.DestroyAsync(new DeletionParams(publicId));
            }

            var success = await _productService.DeleteProductAsync(id, userId);
            if (!success)
            {
                return BadRequest(new { Message = "Failed to delete product" });
            }

            return Ok(new { Message = "Product deleted successfully" });
        }
        [HttpGet("seller-products")]
        [Authorize]
        public async Task<IActionResult> GetProductsBySeller([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Gọi service để lấy sản phẩm theo sellerId từ token
                var products = await _productService.GetProductsBySellerIdAsync(page, pageSize);
                var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);

                return Ok(new
                {
                    Data = productDTOs,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = await _productService.GetTotalProductsCountAsync()  // Hoặc có thể lấy tổng số sản phẩm của seller nếu cần
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }

}
