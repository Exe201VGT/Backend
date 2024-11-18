using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Response
{
	public class LoginResponseModel
	{
		public string Token { get; set; }
		public AccountResponseModel Account { get; set; }
	}
}
