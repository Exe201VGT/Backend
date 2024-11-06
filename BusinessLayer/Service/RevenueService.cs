using BOs.Models;
using BusinessLayer.Modal.Response;
using BusinessLayer.Service.Interface;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class RevenueService : IRevenueService
    {
        private readonly IRevenueRepository _revenueRepository;

        public RevenueService(IRevenueRepository revenueRepository)
        {
            _revenueRepository = revenueRepository;
        }

        public async Task<RevenueSummaryDTO> GetRevenueByDayAsync(int sellerId, DateTime date)
        {
            var revenues = await _revenueRepository.GetRevenueByDayAsync(sellerId, date);
            return MapToRevenueSummary(revenues);
        }

        public async Task<RevenueSummaryDTO> GetRevenueByMonthAsync(int sellerId, int year, int month)
        {
            var revenues = await _revenueRepository.GetRevenueByMonthAsync(sellerId, year, month);
            return MapToRevenueSummary(revenues);
        }

        public async Task<RevenueSummaryDTO> GetRevenueByYearAsync(int sellerId, int year)
        {
            var revenues = await _revenueRepository.GetRevenueByYearAsync(sellerId, year);
            return MapToRevenueSummary(revenues);
        }

        public async Task<RevenueSummaryDTO> GetRevenueByRangeAsync(int sellerId, DateTime startDate, DateTime endDate)
        {
            var revenues = await _revenueRepository.GetRevenueByRangeAsync(sellerId, startDate, endDate);
            return MapToRevenueSummary(revenues);
        }

        private RevenueSummaryDTO MapToRevenueSummary(IEnumerable<Revenue> revenues)
        {
            var revenueDTOs = revenues.Select(r => new RevenueDTO
            {
                RevenueId = r.RevenueId,
                TotalAmount = r.TotalAmount,
                RevenueDate = r.RevenueDate,
                Status = r.Status,
                SellerId = r.SellerId
            });

            return new RevenueSummaryDTO
            {
                TotalRevenue = revenueDTOs.Sum(r => r.TotalAmount ?? 0),
                OrderCount = revenueDTOs.Count(),
                Details = revenueDTOs.ToList()
            };
        }
    }
}
