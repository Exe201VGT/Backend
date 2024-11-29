using AutoMapper;
using BOs.Models;
using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace VietNongAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService; // Đảm bảo chúng ta có IUserService để lấy userId từ token

        public CartController(ICartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }

        [HttpGet("cart")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                // Lấy userId từ token
                int userId = _userService.GetUserIdFromToken();

                var cart = await _cartService.GetCartAsync();
                if (cart == null)
                {
                    return NotFound(new { Message = "Cart not found" });
                }

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // Thêm sản phẩm vào giỏ hàng
        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddToCartDTO addToCartDTO)
        {
            try
            {
                // Gọi service để thêm sản phẩm vào giỏ hàng
                var cartItemDTO = await _cartService.AddItemToCartAsync(addToCartDTO.ProductId, addToCartDTO.Quantity);
                return Ok(cartItemDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPut("update-item/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] int quantity)
        {
            try
            {
                // Lấy userId từ token
                int userId = _userService.GetUserIdFromToken();

                var cartItem = await _cartService.UpdateItemQuantityAsync(cartItemId, quantity);
                if (cartItem == null)
                {
                    return BadRequest(new { Message = "Failed to update product to cart" });
                }

                return Ok(cartItem);
                
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpDelete("remove-item/{cartItemId}")]
        public async Task<IActionResult> RemoveItemFromCart(int cartItemId)
        {
            try
            {
                // Lấy userId từ token
                int userId = _userService.GetUserIdFromToken();

                var result = await _cartService.RemoveItemFromCartAsync(cartItemId);
                if (result)
                    return Ok(new { Message = "Product removed from cart" });

                return BadRequest(new { Message = "Failed to remove product from cart" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }


}



