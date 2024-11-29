using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IMapper mapper, IUserService userService)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<CartDTO> GetCartAsync()
        {
            // Lấy userId từ token
            int userId = _userService.GetUserIdFromToken();

            // Lấy giỏ hàng của người dùng
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return null; // Hoặc có thể trả về một giỏ hàng rỗng, tùy thuộc vào yêu cầu của bạn
            }

            // Chuyển đổi sang DTO
            var cartDTO = _mapper.Map<CartDTO>(cart);

            // Trả về giỏ hàng
            return cartDTO;
        }

        public async Task<CartItemDTO> AddItemToCartAsync(int productId, int quantity)
        {
            // Lấy userId từ token thông qua phương thức GetUserIdFromToken
            int userId = _userService.GetUserIdFromToken();

            // Lấy giỏ hàng của người dùng, nếu có
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            // Nếu người dùng chưa có giỏ hàng, tạo mới giỏ hàng
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Thêm giỏ hàng mới vào cơ sở dữ liệu
                await _cartRepository.AddCartAsync(cart);
            }
            Console.WriteLine($"UserId: {userId}");
            // Thêm sản phẩm vào giỏ hàng
            var cartItem = await _cartRepository.AddItemToCartAsync(cart.CartId, productId, quantity);

            // Trả về CartItemDTO
            return _mapper.Map<CartItemDTO>(cartItem);
        }




        public async Task<CartItemDTO> UpdateItemQuantityAsync(int cartItemId, int quantity)
        {
            // Lấy userId từ token
            int userId = _userService.GetUserIdFromToken();

            // Lấy giỏ hàng của người dùng
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            // Kiểm tra xem giỏ hàng có tồn tại không
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found for the user.");
            }

            // Cập nhật số lượng sản phẩm trong giỏ hàng
            var cartItem = await _cartRepository.UpdateItemQuantityAsync(cart.CartId, cartItemId, quantity);

            // Trả về CartItemDTO
            return _mapper.Map<CartItemDTO>(cartItem);
        }


        public async Task<bool> RemoveItemFromCartAsync(int cartItemId)
        {
            // Lấy userId từ token
            int userId = _userService.GetUserIdFromToken();

            // Lấy giỏ hàng của người dùng
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            // Kiểm tra xem giỏ hàng có tồn tại không
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found for the user.");
            }

            // Xóa sản phẩm khỏi giỏ hàng
            return await _cartRepository.RemoveItemFromCartAsync(cart.CartId, cartItemId);
        }

    }

}
