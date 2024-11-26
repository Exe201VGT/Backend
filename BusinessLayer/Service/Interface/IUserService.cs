using BOs.Models;
using BusinessLayer.Modal.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> UpdateUserStatusAsync(int userId, string status);
        Task<bool> DeleteUserAsync(int userId);
        int GetUserIdFromToken();
        Task<bool> UpdateUserRoleAsync(int userId, int newRoleId);
    }
}
