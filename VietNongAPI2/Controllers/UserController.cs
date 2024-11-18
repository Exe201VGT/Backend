using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BusinessLayer.Service.Interface;
using BOs.Models;
using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            if (user == null)
                return NotFound(new { Message = "User not found" });

            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }

        // Lấy thông tin người dùng theo username
        [HttpGet("username/{username}")]
        [Authorize] // Yêu cầu người dùng phải đăng nhập
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            return Ok(user);
        }

        // Cập nhật profile người dùng
        [HttpPut("profile")]
        [Authorize] // Yêu cầu người dùng phải đăng nhập
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateDTO userProfileUpdateDto)
        {
            var userId = GetUserIdFromToken();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tìm user hiện tại
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = "User not found" });

            // Cập nhật các thông tin profile (trừ UserId)
            _mapper.Map(userProfileUpdateDto, user);

            await _userService.UpdateUserAsync(user);

            return Ok(new { Message = "Profile updated successfully" });
        }

        // Cập nhật trạng thái người dùng (kích hoạt hoặc khóa tài khoản)
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "admin")] // Yêu cầu quyền quản trị viên
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] UserStatusUpdateDTO statusUpdateDto)
        {
            if (id != statusUpdateDto.UserId)
                return BadRequest(new { Message = "User ID mismatch" });

            var result = await _userService.UpdateUserStatusAsync(id, statusUpdateDto.Status);
            if (!result)
                return NotFound(new { Message = "User not found" });

            return Ok(new { Message = "User status updated successfully" });
        }

        // Xóa người dùng (chỉ dành cho quản trị viên)
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound(new { Message = "User not found" });

            return Ok(new { Message = "User deleted successfully" });
        }

        // Xem profile người dùng
        [HttpGet("profile")]
        [Authorize] // Yêu cầu người dùng phải đăng nhập
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile()
        {
            var userId = GetUserIdFromToken();
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound(new { Message = "User not found" });

            var userProfileDTO = _mapper.Map<UserProfileDTO>(user);
            return Ok(userProfileDTO);
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }
            return int.Parse(userIdClaim);
        }
    }
}
