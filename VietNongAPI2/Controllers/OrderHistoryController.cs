using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BOs.Models;
using BusinessLayer.Service.Interface;
using BusinessLayer.Modal.Response;

namespace VietNongAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHistoryController : ControllerBase
    {
        private readonly IOrderHistoryService _orderHistoryService;
        private readonly IMapper _mapper;

        public OrderHistoryController(IOrderHistoryService orderHistoryService, IMapper mapper)
        {
            _orderHistoryService = orderHistoryService;
            _mapper = mapper;
        }

        // Lấy toàn bộ lịch sử mua hàng
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderHistoryDTO>>> GetAllOrderHistories()
        {
            var orderHistories = await _orderHistoryService.GetAllOrderHistoriesAsync();
            var orderHistoryDTOs = _mapper.Map<IEnumerable<OrderHistoryDTO>>(orderHistories);
            return Ok(orderHistoryDTOs);
        }

        // Lấy lịch sử mua hàng theo ID người dùng
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderHistoryDTO>>> GetOrderHistoriesByUserId(int userId)
        {
            var orderHistories = await _orderHistoryService.GetOrderHistoriesByUserIdAsync(userId);
            if (orderHistories == null || !orderHistories.Any()) return NotFound();

            var orderHistoryDTOs = _mapper.Map<IEnumerable<OrderHistoryDTO>>(orderHistories);
            return Ok(orderHistoryDTOs);
        }

        // Lấy lịch sử mua hàng theo ID đơn hàng (nếu cần)
        [HttpGet("Order/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderHistoryDTO>>> GetOrderHistoriesByOrderId(int orderId)
        {
            var orderHistories = await _orderHistoryService.GetOrderHistoriesByOrderIdAsync(orderId);
            if (orderHistories == null || !orderHistories.Any()) return NotFound();

            var orderHistoryDTOs = _mapper.Map<IEnumerable<OrderHistoryDTO>>(orderHistories);
            return Ok(orderHistoryDTOs);
        }
    }
}
