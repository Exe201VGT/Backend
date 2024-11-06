using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BOs.Models;
using BusinessLayer.Service.Interface;
using BusinessLayer.Modal.Request;
using BusinessLayer.Modal.Response;

namespace VietNongAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMapper _mapper;

        public OrderDetailController(IOrderDetailService orderDetailService, IMapper mapper)
        {
            _orderDetailService = orderDetailService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailDTO>>> GetAllOrderDetails()
        {
            var orderDetails = await _orderDetailService.GetAllOrderDetailsAsync();
            var orderDetailDTOs = _mapper.Map<IEnumerable<OrderDetailDTO>>(orderDetails);
            return Ok(orderDetailDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailDTO>> GetOrderDetailById(int id)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
            if (orderDetail == null) return NotFound();

            var orderDetailDTO = _mapper.Map<OrderDetailDTO>(orderDetail);
            return Ok(orderDetailDTO);
        }

        [HttpGet("Order/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderDetailDTO>>> GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderIdAsync(orderId);
            var orderDetailDTOs = _mapper.Map<IEnumerable<OrderDetailDTO>>(orderDetails);
            return Ok(orderDetailDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDetailDTO>> CreateOrderDetail([FromBody] OrderDetailCreateDTO orderDetailCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderDetail = _mapper.Map<OrderDetail>(orderDetailCreateDto);
            var orderDetailId = await _orderDetailService.CreateOrderDetailAsync(orderDetail);

            if (orderDetailId <= 0)
            {
                return BadRequest(new { Message = "Failed to create order detail" });
            }

            var createdOrderDetail = await _orderDetailService.GetOrderDetailByIdAsync(orderDetailId);
            var createdOrderDetailDto = _mapper.Map<OrderDetailDTO>(createdOrderDetail);

            return CreatedAtAction(nameof(GetOrderDetailById), new { id = orderDetailId }, createdOrderDetailDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetailUpdateDTO orderDetailUpdateDto)
        {
            if (id != orderDetailUpdateDto.OrderDetailId) return BadRequest();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderDetail = _mapper.Map<OrderDetail>(orderDetailUpdateDto);
            await _orderDetailService.UpdateOrderDetailAsync(orderDetail);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            await _orderDetailService.DeleteOrderDetailAsync(id);
            return NoContent();
        }
    }
}
