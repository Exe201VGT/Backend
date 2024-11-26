using BOs.Models;
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
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISellerService _sellerService;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository, ISellerService sellerService)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _sellerService = sellerService;
        }

        // Lấy giỏ hàng của người dùng
        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        // Thêm sản phẩm vào giỏ hàng
        public async Task<CartItemDTO> AddItemToCartAsync(int userId, CartItem cartItem)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CreatedAt = DateTime.UtcNow };
                await _cartRepository.CreateCartAsync(cart);
            }

            // Kiểm tra sự tồn tại của sản phẩm
            var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            // Thêm sản phẩm vào giỏ hàng
            cartItem.CartId = cart.CartId;
            cartItem.CreatedAt = DateTime.UtcNow;

            // Lưu vào cơ sở dữ liệu
            var addedCartItem = await _cartRepository.AddCartItemAsync(cartItem);

            // Tạo CartItemDTO để trả về thông tin giỏ hàng bao gồm thông tin sản phẩm
            var cartItemDTO = new CartItemDTO
            {
                CartItemId = addedCartItem.CartItemId,
                ProductId = addedCartItem.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = addedCartItem.Quantity,
                ProductImage = product.ProductImage // Lấy đường dẫn hình ảnh từ sản phẩm
            };

            return cartItemDTO;
        }


        // Cập nhật sản phẩm trong giỏ hàng
        public async Task<bool> UpdateCartItemAsync(int userId, CartItem cartItem)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            var existingCartItem = await _cartRepository.GetCartItemByIdAsync(cartItem.CartItemId);
            if (existingCartItem == null || existingCartItem.CartId != cart.CartId)
            {
                throw new InvalidOperationException("Cart item not found.");
            }

            existingCartItem.Quantity = cartItem.Quantity;
            return await _cartRepository.UpdateCartItemAsync(existingCartItem);
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public async Task<bool> RemoveItemFromCartAsync(int userId, int cartItemId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            return await _cartRepository.RemoveCartItemAsync(cartItemId);
        }

        // Xóa tất cả sản phẩm trong giỏ hàng (dùng khi thanh toán)
        public async Task<bool> ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            return await _cartRepository.ClearCartAsync(cart.CartId);
        }

        // Thanh toán giỏ hàng
        public async Task<bool> CheckoutAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            // Thanh toán và xóa sản phẩm trong giỏ
            return await _cartRepository.CheckoutCartAsync(cart.CartId);
        }
    }

}
