using AutoMapper;
using BOs.Models;
using BusinessLayer.Service.Interface;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ISellerService _sellerService;

        public ProductService(IProductRepository productRepository, ISellerService sellerService)
        {
            _productRepository = productRepository;
            _sellerService = sellerService;
        }

        public async Task<IEnumerable<Product>> GetPagedProductsAsync(int page, int pageSize)
        {
            return await _productRepository.GetPagedProductsAsync(page, pageSize);
        }

        public async Task<int> GetTotalProductsCountAsync()
        {
            return await _productRepository.GetTotalProductsCountAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _productRepository.GetProductByIdAsync(productId);
        }

        public async Task<Product> CreateProductAsync(Product product, int userId)
        {
            // Sử dụng GetSellerByUserIdAsync để lấy Seller
            var seller = await _sellerService.GetSellerByUserIdAsync(userId);
            if (seller == null)
            {
                throw new InvalidOperationException("User is not a registered seller.");
            }

            // Lấy SellerId từ Seller
            product.SellerId = seller.SellerId;
            product.CreatedAt = DateTime.UtcNow;

            await _productRepository.CreateProductAsync(product);
            return product;
        }

        public async Task<bool> UpdateProductAsync(Product updatedProduct, int userId)
        {
            // Lấy thông tin sản phẩm hiện tại
            var existingProduct = await _productRepository.GetProductByIdAsync(updatedProduct.ProductId);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            // Kiểm tra quyền sở hữu sản phẩm
            var seller = await _sellerService.GetSellerByUserIdAsync(userId);
            if (seller == null || existingProduct.SellerId != seller.SellerId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this product.");
            }

            // Cập nhật thông tin sản phẩm
            existingProduct.Name = updatedProduct.Name ?? existingProduct.Name;
            existingProduct.CategoryId = updatedProduct.CategoryId ?? existingProduct.CategoryId;
            existingProduct.Price = updatedProduct.Price ?? existingProduct.Price;
            existingProduct.Weight = updatedProduct.Weight ?? existingProduct.Weight;
            existingProduct.Description = updatedProduct.Description ?? existingProduct.Description;
            existingProduct.StockQuantity = updatedProduct.StockQuantity ?? existingProduct.StockQuantity;
            existingProduct.CreatedAt = DateTime.UtcNow;

            // Lưu thay đổi
            return await _productRepository.UpdateProductAsync(existingProduct);
        }


        public async Task<bool> DeleteProductAsync(int productId, int userId)
        {
            // Lấy thông tin sản phẩm
            var existingProduct = await _productRepository.GetProductByIdAsync(productId);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            // Kiểm tra quyền sở hữu sản phẩm
            var seller = await _sellerService.GetSellerByUserIdAsync(userId);
            if (seller == null || existingProduct.SellerId != seller.SellerId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this product.");
            }

            // Xóa sản phẩm
            return await _productRepository.DeleteProductAsync(productId);
        }

    }

}
