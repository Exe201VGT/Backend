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

namespace VietNongAPI2.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class SellerController : ODataController
    {
        private readonly ISellerService _sellerService;
        private readonly IMapper _mapper;

        public SellerController(ISellerService sellerService, IMapper mapper)
        {
            _sellerService = sellerService;
            _mapper = mapper;
        }

        // 1. API đăng ký seller - cho phép người dùng đăng ký làm seller
        [HttpPost("register")]
        [Authorize] // Yêu cầu người dùng phải đăng nhập
        public async Task<ActionResult<SellerDTO>> RegisterSeller([FromBody] SellerRegisterDTO sellerRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Lấy UserId từ thông tin xác thực của người dùng hiện tại
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Kiểm tra xem người dùng đã là seller chưa
            var existingSeller = await _sellerService.GetSellerByUserIdAsync(userId);
            if (existingSeller != null)
            {
                return Conflict(new { Message = "User is already registered as a seller." });
            }

            var seller = _mapper.Map<Seller>(sellerRegisterDto);
            seller.UserId = userId; // Gán UserId của người dùng hiện tại vào Seller

            var createdSeller = await _sellerService.RegisterSellerAsync(seller);
            var createdSellerDTO = _mapper.Map<SellerDTO>(createdSeller);

            return CreatedAtAction(nameof(GetSellerById), new { id = createdSellerDTO.SellerId }, createdSellerDTO);
        }

        // 2. Lấy danh sách tất cả seller - chỉ dành cho quản trị viên
        [HttpGet]
        [Authorize(Roles = "admin")] // Yêu cầu quyền quản trị viên
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

        // 4. Cập nhật thông tin seller
        [HttpPut("{id}")]
        [Authorize] // Yêu cầu người dùng phải đăng nhập
        public async Task<IActionResult> UpdateSeller(int id, [FromBody] SellerUpdateDTO sellerUpdateDto)
        {
            // Lấy UserId từ token của người dùng hiện tại
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Kiểm tra quyền truy cập - chỉ cho phép cập nhật nếu là seller hoặc quản trị viên
            var existingSeller = await _sellerService.GetSellerByIdAsync(id);
            if (existingSeller == null)
                return NotFound();

            if (existingSeller.UserId != userId && !User.IsInRole("admin"))
                return Forbid();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Cập nhật các trường cần thiết thay vì ánh xạ toàn bộ
            _mapper.Map(sellerUpdateDto, existingSeller);

            await _sellerService.UpdateSellerAsync(existingSeller);

            return Ok(new { Message = "Update thành công" });
        }

        // 5. Xóa seller - chỉ dành cho quản trị viên
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")] // Yêu cầu quyền quản trị viên
        public async Task<IActionResult> DeleteSeller(int id)
        {
            var result = await _sellerService.DeleteSellerAsync(id);
            if (!result)
                return NotFound();

            return Ok(new { Message = "Delete thành công" });
        }
    }
}
