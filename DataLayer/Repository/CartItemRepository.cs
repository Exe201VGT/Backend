using BOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ICartItemRepository
    {
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId); // Lấy các sản phẩm trong giỏ hàng
        Task<CartItem> GetCartItemByIdAsync(int cartItemId); // Lấy một sản phẩm trong giỏ hàng
        Task<bool> AddCartItemAsync(CartItem cartItem); // Thêm sản phẩm vào giỏ
        Task<bool> UpdateCartItemAsync(CartItem cartItem); // Cập nhật sản phẩm trong giỏ
        Task<bool> RemoveCartItemAsync(int cartItemId); // Xóa sản phẩm trong giỏ
        Task<bool> ClearCartAsync(int cartId); // Xóa tất cả sản phẩm trong giỏ hàng
    }

    public class CartItemRepository : ICartItemRepository
    {
        private readonly VietNongContext _context;

        public CartItemRepository(VietNongContext context)
        {
            _context = context;  // Inject context
        }

        // Lấy các sản phẩm trong giỏ hàng
        public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)  // Lọc theo CartId
                .Include(ci => ci.Product)  // Lấy thông tin sản phẩm trong CartItem
                .ToListAsync();
        }

        // Lấy một sản phẩm trong giỏ hàng
        public async Task<CartItem> GetCartItemByIdAsync(int cartItemId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product) // Lấy thông tin sản phẩm trong CartItem
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        }

        // Thêm sản phẩm vào giỏ hàng
        public async Task<bool> AddCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);  // Thêm CartItem vào context
            return await _context.SaveChangesAsync() > 0;  // Kiểm tra xem có thêm thành công không
        }

        // Cập nhật sản phẩm trong giỏ hàng
        public async Task<bool> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);  // Cập nhật CartItem trong context
            return await _context.SaveChangesAsync() > 0;  // Kiểm tra xem có cập nhật thành công không
        }

        // Xóa sản phẩm trong giỏ hàng
        public async Task<bool> RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await GetCartItemByIdAsync(cartItemId);
            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);  // Xóa CartItem khỏi context
            return await _context.SaveChangesAsync() > 0;  // Kiểm tra xem có xóa thành công không
        }

        // Xóa tất cả sản phẩm trong giỏ hàng
        public async Task<bool> ClearCartAsync(int cartId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);  // Xóa tất cả sản phẩm trong giỏ
            return await _context.SaveChangesAsync() > 0;  // Kiểm tra xem có xóa thành công không
        }
    }

}
