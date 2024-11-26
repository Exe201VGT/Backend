using BOs.Models;
using BusinessLayer.Modal.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface ICartService
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<CartItemDTO> AddItemToCartAsync(int userId, CartItem cartItem);
        Task<bool> UpdateCartItemAsync(int userId, CartItem cartItem);
        Task<bool> RemoveItemFromCartAsync(int userId, int cartItemId);
        Task<bool> ClearCartAsync(int userId);
        Task<bool> CheckoutAsync(int userId);
    }

}
