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
		IEnumerable<User> GetUsers();
		Task<User> GetUserByIdAsync(int id);
		Task CreateUserAsync(User User);
		Task UpdateUserAsync(User User);
		Task DeleteUserAsync(int id);
		Task<bool> UserExistsAsync(int id);
		Task<User> GetUserByUsernameAsync(string username);
		Task<AccountResponseModel> GetUserProfile(int id);
	}
}
