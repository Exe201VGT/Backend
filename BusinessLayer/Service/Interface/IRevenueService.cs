using BusinessLayer.Modal.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Interface
{
    public interface IRevenueService
    {
        Task<RevenueSummaryDTO> GetRevenueByDayAsync(int sellerId, DateTime date);
        Task<RevenueSummaryDTO> GetRevenueByMonthAsync(int sellerId, int year, int month);
        Task<RevenueSummaryDTO> GetRevenueByYearAsync(int sellerId, int year);
        Task<RevenueSummaryDTO> GetRevenueByRangeAsync(int sellerId, DateTime startDate, DateTime endDate);
    }
}
