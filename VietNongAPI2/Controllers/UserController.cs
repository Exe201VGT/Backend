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
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace VietNongAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;
        public UserController(IUserService userService, IMapper mapper, Cloudinary cloudinary)
        {
            _userService = userService;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        // Lấy danh sách người dùng (cho quản trị viên)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(userDTOs);
        }

        // Lấy thông tin chi tiết người dùng theo ID (cho quản trị viên)
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
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
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UserProfileUpdateDTO userProfileUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về lỗi nếu DTO không hợp lệ
            }

            var userId = _userService.GetUserIdFromToken();

            // Tìm user hiện tại
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Upload ảnh lên Cloudinary nếu có
            if (userProfileUpdateDto.ProfileImage != null)
            {
                // Xóa ảnh cũ trên Cloudinary nếu có
                if (!string.IsNullOrEmpty(user.ProfileImage))
                {
                    var publicId = user.ProfileImage.Split('/').Last().Split('.').First();
                    await _cloudinary.DestroyAsync(new DeletionParams(publicId));
                }

                // Upload ảnh mới
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(userProfileUpdateDto.ProfileImage.FileName, userProfileUpdateDto.ProfileImage.OpenReadStream()),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    user.ProfileImage = uploadResult.SecureUrl.ToString(); // Lưu URL ảnh mới
                }
                else
                {
                    return StatusCode((int)uploadResult.StatusCode, new { Message = uploadResult.Error.Message });
                }
            }

            // Cập nhật thông tin khác
            user.Email = userProfileUpdateDto.Email;
            user.FullName = userProfileUpdateDto.FullName;
            user.PhoneNumber = userProfileUpdateDto.PhoneNumber;
            user.Address = userProfileUpdateDto.Address;
            user.DateOfBirth = userProfileUpdateDto.DateOfBirth;
            user.Gender = userProfileUpdateDto.Gender;

            // Gọi service để cập nhật user
            await _userService.UpdateUserAsync(user);

            return Ok(new { Message = "Profile updated successfully", ProfileImageUrl = user.ProfileImage });
        }


        // Cập nhật trạng thái người dùng (kích hoạt hoặc khóa tài khoản)
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")] // Yêu cầu quyền quản trị viên
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
        [Authorize(Roles = "Admin")]
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
            var userId = _userService.GetUserIdFromToken();
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound(new { Message = "User not found" });

            var userProfileDTO = _mapper.Map<UserProfileDTO>(user);
            return Ok(userProfileDTO);
        }

    }
}
