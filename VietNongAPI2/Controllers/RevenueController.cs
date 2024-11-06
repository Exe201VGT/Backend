using BusinessLayer.Modal.Response;
using BusinessLayer.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VietNongAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueService _revenueService;

        public RevenueController(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }

        [HttpGet("GetByDay")]
        public async Task<ActionResult<RevenueSummaryDTO>> GetRevenueByDay(int sellerId, DateTime date)
        {
            var revenueSummary = await _revenueService.GetRevenueByDayAsync(sellerId, date);
            return Ok(revenueSummary);
        }

        [HttpGet("GetByMonth")]
        public async Task<ActionResult<RevenueSummaryDTO>> GetRevenueByMonth(int sellerId, int year, int month)
        {
            var revenueSummary = await _revenueService.GetRevenueByMonthAsync(sellerId, year, month);
            return Ok(revenueSummary);
        }

        [HttpGet("GetByYear")]
        public async Task<ActionResult<RevenueSummaryDTO>> GetRevenueByYear(int sellerId, int year)
        {
            var revenueSummary = await _revenueService.GetRevenueByYearAsync(sellerId, year);
            return Ok(revenueSummary);
        }

        [HttpGet("GetByRange")]
        public async Task<ActionResult<RevenueSummaryDTO>> GetRevenueByRange(int sellerId, DateTime startDate, DateTime endDate)
        {
            var revenueSummary = await _revenueService.GetRevenueByRangeAsync(sellerId, startDate, endDate);
            return Ok(revenueSummary);
        }
    }
}
