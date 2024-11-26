using BOs.Models;
using BusinessLayer.Modal.Response;
using BusinessLayer.Service.Interface;
using DataLayer.Repository;
using DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // Kiểm tra ngày sinh (trên 18 tuổi)
            if (user.DateOfBirth.HasValue)
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow); // Chuyển đổi DateTime thành DateOnly
                var age = today.Year - user.DateOfBirth.Value.Year;

                // Nếu ngày sinh + tuổi < hôm nay, giảm tuổi xuống 1
                if (user.DateOfBirth.Value.AddYears(age) > today)
                {
                    age--;
                }

                if (age < 18)
                {
                    throw new ArgumentException("User must be at least 18 years old.");
                }
            }

            // Kiểm tra email trùng
            if (await _userRepository.IsEmailTaken(user.Email, user.UserId))
            {
                throw new ArgumentException("Email is already in use.");
            }

            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, string status)
        {
            return await _userRepository.UpdateUserStatusAsync(userId, status);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userRepository.DeleteUserAsync(userId);
        }
        public int GetUserIdFromToken()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }

            return int.Parse(userIdClaim);
        }
        public async Task<bool> UpdateUserRoleAsync(int userId, int newRoleId)
        {
            // Kiểm tra userId có tồn tại không
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            // Cập nhật RoleId
            user.RoleId = newRoleId;
            user.UpdatedAt = DateTime.UtcNow; // Cập nhật thời gian sửa đổi

            // Lưu thay đổi
            return await _userRepository.UpdateUserAsync(user);
        }


    }
}