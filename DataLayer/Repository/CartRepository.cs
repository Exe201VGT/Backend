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
        Task<Cart> GetCartByUserIdAsync(int userId); // Lấy giỏ hàng theo userId
        Task<CartItem> AddItemToCartAsync(int userId, int productId, int quantity); // Thêm sản phẩm vào giỏ hàng
        Task<bool> SaveChangesAsync(); // Lưu thay đổi
        Task AddCartAsync(Cart cart); // Thêm giỏ hàng mới
        Task<CartItem> UpdateItemQuantityAsync(int cartId, int cartItemId, int quantity); // Cập nhật số lượng sản phẩm
        Task<bool> RemoveItemFromCartAsync(int cartId, int cartItemId); // Xóa sản phẩm khỏi giỏ hàng
        Task<CartItem> GetCartItemByCartIdAndProductIdAsync(int cartId, int productId);
    }

    public class CartRepository : ICartRepository
    {
        private readonly VietNongContext _context;

        public CartRepository(VietNongContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)          // Bao gồm các CartItem
                .ThenInclude(ci => ci.Product)      // Bao gồm thông tin sản phẩm trong mỗi CartItem
                .FirstOrDefaultAsync(c => c.UserId == userId); // Tìm giỏ hàng của người dùng theo userId
        }


        // Thêm sản phẩm vào giỏ hàng
        public async Task<CartItem> AddItemToCartAsync(int cartId, int productId, int quantity)
        {
            // Kiểm tra xem người dùng đã có giỏ hàng chưa
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);

            // Nếu không có giỏ hàng, tạo mới giỏ hàng
            //if (cart == null)
            //{
            //    cart = new Cart
            //    {
            //        UserId = userId,
            //        CreatedAt = DateTime.UtcNow,
            //        UpdatedAt = DateTime.UtcNow
            //    };

            //    await _context.Carts.AddAsync(cart);
            //    await SaveChangesAsync();  // Lưu giỏ hàng mới vào cơ sở dữ liệu
            //}

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId && ci.ProductId == productId);

            if (existingCartItem != null)
            {
                // Nếu có, chỉ cập nhật số lượng
                existingCartItem.Quantity += quantity;
                existingCartItem.UpdatedAt = DateTime.UtcNow;
                await SaveChangesAsync();
                return existingCartItem;
            }
            else
            {
                // Nếu chưa có, thêm mới sản phẩm vào giỏ hàng
                var product = await _context.Products.FindAsync(productId);
                if (product == null) return null; // Nếu sản phẩm không tồn tại

                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.CartItems.AddAsync(cartItem);
                await SaveChangesAsync();  // Lưu thay đổi
                return cartItem;
            }
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        public async Task<CartItem> UpdateItemQuantityAsync(int cartId, int cartItemId, int quantity)
        {
            // Tìm kiếm CartItem với cartId và cartItemId
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.CartItemId == cartItemId);

            // Nếu không tìm thấy CartItem, trả về null
            if (cartItem == null) return null;

            // Cập nhật số lượng và thời gian cập nhật
            cartItem.Quantity = quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;

            // Lưu thay đổi vào cơ sở dữ liệu và trả về CartItem đã cập nhật
            await SaveChangesAsync();

            // Trả về đối tượng CartItem đã được cập nhật
            return cartItem;
        }


        // Xóa sản phẩm khỏi giỏ hàng
        public async Task<bool> RemoveItemFromCartAsync(int cartId, int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.CartItemId == cartItemId);

            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);
            return await SaveChangesAsync();
        }

        // Lưu thay đổi vào cơ sở dữ liệu
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        // Thêm giỏ hàng mới
        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await SaveChangesAsync();
        }
        public async Task<CartItem> GetCartItemByCartIdAndProductIdAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

    }


}
