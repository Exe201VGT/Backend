using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BusinessLayer.Service.Interface;
using BOs.Models;
using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;
using Microsoft.AspNetCore.Authorization;

namespace VietNongAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // Lấy danh sách người dùng (cho quản trị viên)
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(userDTOs);
        }

        // Lấy thông tin chi tiết người dùng theo ID (cho quản trị viên)
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Cập nhật thông tin người dùng (cho quản trị viên)
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userUpdateDto)
        {
            if (id != userUpdateDto.UserId) return BadRequest();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(userUpdateDto);
            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        // Cập nhật trạng thái người dùng (kích hoạt hoặc khóa tài khoản)
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] UserStatusUpdateDTO statusUpdateDto)
        {
            if (id != statusUpdateDto.UserId) return BadRequest();

            var result = await _userService.UpdateUserStatusAsync(id, statusUpdateDto.Status);
            if (!result) return NotFound();

            return NoContent();
        }

        // Xóa người dùng (chỉ dành cho quản trị viên)
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        // Xem profile người dùng
        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile()
        {
            var userId = GetUserIdFromToken();
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null) return NotFound();

            var userProfileDTO = _mapper.Map<UserProfileDTO>(user);
            return Ok(userProfileDTO);
        }

        // Cập nhật profile người dùng
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateDTO userProfileUpdateDto)
        {
            var userId = GetUserIdFromToken();
            if (userId != userProfileUpdateDto.UserId) return BadRequest();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(userProfileUpdateDto);
            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        private int GetUserIdFromToken()
        {
            return int.Parse(User.FindFirst("UserId").Value);
        }
    }
}
