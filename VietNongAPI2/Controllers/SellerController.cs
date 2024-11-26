using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Service.Interface;
using BOs.Models;
using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace VietNongAPI2.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class SellerController : ODataController
    {
        private readonly ISellerService _sellerService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly Cloudinary _cloudinary;
        public SellerController(ISellerService sellerService, IMapper mapper, IUserService userService, Cloudinary cloudinary)
        {
            _sellerService = sellerService;
            _mapper = mapper;
            _userService = userService;
            _cloudinary = cloudinary;
        }

        // 1. API đăng ký seller - cho phép người dùng đăng ký làm seller
        [HttpPost("register")]
        [Authorize] // Yêu cầu người dùng phải đăng nhập
        public async Task<ActionResult<SellerDTO>> RegisterSeller([FromForm] SellerRegisterDTO sellerRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // 1. Lấy UserId từ thông tin xác thực của người dùng hiện tại
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                // 2. Kiểm tra xem người dùng đã đăng ký seller chưa
                var existingSeller = await _sellerService.GetSellerByUserIdAsync(userId);
                if (existingSeller != null)
                {
                    return Conflict(new { Message = "User is already registered as a seller." });
                }

                // 3. Xử lý hình ảnh (nếu có)
                string? shopImageUrl = null;
                if (sellerRegisterDto.ShopImage != null)
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(sellerRegisterDto.ShopImage.FileName, sellerRegisterDto.ShopImage.OpenReadStream()),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        shopImageUrl = uploadResult.SecureUrl.ToString();
                    }
                    else
                    {
                        return StatusCode((int)uploadResult.StatusCode, new { Message = "Failed to upload shop image." });
                    }
                }

                // 4. Ánh xạ từ DTO sang model Seller
                var seller = _mapper.Map<Seller>(sellerRegisterDto);
                seller.UserId = userId;
                seller.ShopImage = shopImageUrl; // Gán URL hình ảnh (nếu có)

                // 5. Lưu seller vào cơ sở dữ liệu
                var createdSeller = await _sellerService.RegisterSellerAsync(seller);

                // 6. Cập nhật RoleId của User thành "Seller" (RoleId = 2)
                var roleUpdated = await _userService.UpdateUserRoleAsync(userId, 3);
                if (!roleUpdated)
                {
                    // Xóa seller nếu cập nhật RoleId thất bại
                    await _sellerService.DeleteSellerAsync(createdSeller.SellerId);
                    return StatusCode(500, new { Message = "Failed to update user role. Registration reverted." });
                }

                // 7. Trả về seller vừa được tạo
                var createdSellerDTO = _mapper.Map<SellerDTO>(createdSeller);
                return CreatedAtAction(nameof(GetSellerById), new { id = createdSellerDTO.SellerId }, createdSellerDTO);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                // _logger.LogError(ex, "Error occurred while registering seller.");
                return StatusCode(500, new { Message = "An unexpected error occurred during registration." });
            }
        }



        // 2. Lấy danh sách tất cả seller - chỉ dành cho quản trị viên
        [HttpGet]
        [Authorize(Roles = "Admin")] // Yêu cầu quyền quản trị viên
        public async Task<ActionResult<IEnumerable<SellerDTO>>> GetAllSellers()
        {
            var sellers = await _sellerService.GetAllSellersAsync();
            var sellerDTOs = _mapper.Map<IEnumerable<SellerDTO>>(sellers);
            return Ok(sellerDTOs);
        }

        // 3. Lấy chi tiết seller theo ID
        [HttpGet("{id}")]
        [Authorize] // Yêu cầu người dùng phải đăng nhập
        public async Task<ActionResult<SellerDTO>> GetSellerById(int id)
        {
            var seller = await _sellerService.GetSellerByIdAsync(id);
            if (seller == null)
                return NotFound();

            var sellerDTO = _mapper.Map<SellerDTO>(seller);
            return Ok(sellerDTO);
        }

        [HttpPut("{id}")]
        [Authorize] // Yêu cầu người dùng phải đăng nhập
        public async Task<IActionResult> UpdateSeller(int id, [FromForm] SellerUpdateDTO sellerUpdateDto)
        {
            // Lấy UserId từ token của người dùng hiện tại
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Kiểm tra quyền truy cập - chỉ cho phép cập nhật nếu là seller hoặc quản trị viên
            var existingSeller = await _sellerService.GetSellerByIdAsync(id);
            if (existingSeller == null)
                return NotFound();

            if (existingSeller.UserId != userId && !User.IsInRole("Seller"))
                return Forbid();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? shopImageUrl = null;

            // Xử lý hình ảnh
            if (sellerUpdateDto.ShopImage != null)
            {
                // Xóa hình ảnh cũ nếu tồn tại
                if (!string.IsNullOrEmpty(existingSeller.ShopImage))
                {
                    var publicId = existingSeller.ShopImage.Split('/').Last().Split('.').First();
                    await _cloudinary.DestroyAsync(new DeletionParams(publicId));
                }

                // Upload hình ảnh mới
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(sellerUpdateDto.ShopImage.FileName, sellerUpdateDto.ShopImage.OpenReadStream()),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    shopImageUrl = uploadResult.SecureUrl.ToString();
                }
                else
                {
                    return StatusCode((int)uploadResult.StatusCode, new { Message = "Failed to upload shop image." });
                }
            }

            // Cập nhật thông tin Seller
            _mapper.Map(sellerUpdateDto, existingSeller);

            // Cập nhật URL hình ảnh nếu có
            if (!string.IsNullOrEmpty(shopImageUrl))
            {
                existingSeller.ShopImage = shopImageUrl;
            }

            await _sellerService.UpdateSellerAsync(existingSeller);

            return Ok(new { Message = "Update thành công" });
        }


        // 5. Xóa seller - chỉ dành cho quản trị viên
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Yêu cầu quyền quản trị viên
        public async Task<IActionResult> DeleteSeller(int id)
        {
            var result = await _sellerService.DeleteSellerAsync(id);
            if (!result)
                return NotFound();

            return Ok(new { Message = "Delete thành công" });
        }


    }
}
