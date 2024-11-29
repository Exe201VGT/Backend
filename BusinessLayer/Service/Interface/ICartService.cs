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
        Task<CartDTO> GetCartAsync();
        Task<CartItemDTO> AddItemToCartAsync(int productId, int quantity);
        Task<CartItemDTO> UpdateItemQuantityAsync(int cartItemId, int quantity);
        Task<bool> RemoveItemFromCartAsync(int cartItemId);
    }

}
