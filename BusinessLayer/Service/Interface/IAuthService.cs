using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
	public interface IAuthServices
	{
		Task<BaseResponseForLogin<LoginResponseModel>> AuthenticateAsync(string username, string password);

		Task<BaseResponse<TokenModel>> RegisterAsync(RegisterDTO registerDTO);
		string HashPassword(string password);
		bool VerifyPassword(string password, string hashedPassword);
	}

}
