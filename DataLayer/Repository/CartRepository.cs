using BOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem> GetCartItemByIdAsync(int cartItemId);
        Task<bool> UpdateCartItemAsync(CartItem cartItem);
        Task<bool> RemoveCartItemAsync(int cartItemId);
        Task<bool> ClearCartAsync(int cartId);
        Task<bool> CheckoutCartAsync(int cartId);
        Task CreateCartAsync(Cart cart);
    }
    public class CartRepository : ICartRepository
    {
        private readonly VietNongContext _context;

        public CartRepository(VietNongContext context)
        {
            _context = context;
        }

        // Lấy giỏ hàng của người dùng
        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // Thêm sản phẩm vào giỏ hàng
        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        // Lấy sản phẩm trong giỏ hàng theo ID
        public async Task<CartItem> GetCartItemByIdAsync(int cartItemId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        }

        // Cập nhật sản phẩm trong giỏ hàng
        public async Task<bool> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            return await _context.SaveChangesAsync() > 0;
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public async Task<bool> RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);
            return await _context.SaveChangesAsync() > 0;
        }

        // Xóa tất cả sản phẩm trong giỏ hàng
        public async Task<bool> ClearCartAsync(int cartId)
        {
            var cartItems = _context.CartItems.Where(ci => ci.CartId == cartId);
            _context.CartItems.RemoveRange(cartItems);
            return await _context.SaveChangesAsync() > 0;
        }

        // Thanh toán giỏ hàng
        public async Task<bool> CheckoutCartAsync(int cartId)
        {
            var cartItems = _context.CartItems.Where(ci => ci.CartId == cartId).ToList();
            _context.CartItems.RemoveRange(cartItems);
            return await _context.SaveChangesAsync() > 0;
        }

        // Tạo giỏ hàng mới
        public async Task CreateCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }
    }

}
