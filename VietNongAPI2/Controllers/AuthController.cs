using BusinessLayer.Modal.Request;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VietNongAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;

        public AuthController(IAuthServices authServices)
        {
            _authService = authServices;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO model)
        {
            var result = await _authService.AuthenticateAsync(model.Username, model.Password);

            return StatusCode((int)result.Code, result);
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDTO model)
        {
            // Implement user registration logic here

            // Once the user is registered, generate JWT token
            //return Ok(_authService.RegisterAsync(model).Result);
            var result = _authService.RegisterAsync(model).Result;
            return StatusCode((int)result.Code, result);
        }
    }
}
