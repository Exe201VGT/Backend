using BOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> UpdateUserStatusAsync(int userId, string status);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> IsEmailTaken(string email, int? userId = null);

    }
    public class UserRepository : IUserRepository
    {
        private readonly VietNongContext _context;

        public UserRepository(VietNongContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role)  // Bao gồm thông tin Role
                .FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, string status)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Status = status;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> IsEmailTaken(string email, int? userId = null)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email && (!userId.HasValue || u.UserId != userId));
        }

    }
}
