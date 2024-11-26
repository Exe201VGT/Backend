using BOs.Models;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VietNongAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Lấy giỏ hàng của người dùng
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null) return NotFound(new { Message = "Cart not found" });
            return Ok(cart);
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost("user/{userId}/items")]
        public async Task<IActionResult> AddItemToCart(int userId, [FromBody] CartItem cartItem)
        {
            var addedCartItemDTO = await _cartService.AddItemToCartAsync(userId, cartItem);
            return CreatedAtAction(nameof(GetCartByUserId), new { userId = userId }, addedCartItemDTO);
        }


        // Cập nhật sản phẩm trong giỏ hàng
        [HttpPut("user/{userId}/items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int userId, int cartItemId, [FromBody] CartItem cartItem)
        {
            cartItem.CartItemId = cartItemId;
            var success = await _cartService.UpdateCartItemAsync(userId, cartItem);
            if (!success) return NotFound(new { Message = "Cart item not found" });
            return Ok();
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpDelete("user/{userId}/items/{cartItemId}")]
        public async Task<IActionResult> RemoveItemFromCart(int userId, int cartItemId)
        {
            var success = await _cartService.RemoveItemFromCartAsync(userId, cartItemId);
            if (!success) return NotFound(new { Message = "Cart item not found" });
            return NoContent();
        }

        // Thanh toán giỏ hàng
        [HttpPost("user/{userId}/checkout")]
        public async Task<IActionResult> Checkout(int userId)
        {
            var success = await _cartService.CheckoutAsync(userId);
            if (!success) return BadRequest(new { Message = "Checkout failed" });
            return Ok(new { Message = "Checkout successful" });
        }
    }

}
