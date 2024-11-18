using BOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IRevenueRepository
    {
        Task<IEnumerable<Revenue>> GetRevenueByDayAsync(int sellerId, DateTime date);
        Task<IEnumerable<Revenue>> GetRevenueByMonthAsync(int sellerId, int year, int month);
        Task<IEnumerable<Revenue>> GetRevenueByYearAsync(int sellerId, int year);
        Task<IEnumerable<Revenue>> GetRevenueByRangeAsync(int sellerId, DateTime startDate, DateTime endDate);
    }

    public class RevenueRepository : IRevenueRepository
    {
        private readonly VietNongContext _context;

        public RevenueRepository(VietNongContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Revenue>> GetRevenueByDayAsync(int sellerId, DateTime date)
        {
            return await _context.Revenues
                .Where(r => r.SellerId == sellerId && r.RevenueDate.Value.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Revenue>> GetRevenueByMonthAsync(int sellerId, int year, int month)
        {
            return await _context.Revenues
                .Where(r => r.SellerId == sellerId && r.RevenueDate.Value.Year == year && r.RevenueDate.Value.Month == month)
                .ToListAsync();
        }

        public async Task<IEnumerable<Revenue>> GetRevenueByYearAsync(int sellerId, int year)
        {
            return await _context.Revenues
                .Where(r => r.SellerId == sellerId && r.RevenueDate.Value.Year == year)
                .ToListAsync();
        }

        public async Task<IEnumerable<Revenue>> GetRevenueByRangeAsync(int sellerId, DateTime startDate, DateTime endDate)
        {
            return await _context.Revenues
                .Where(r => r.SellerId == sellerId && r.RevenueDate.Value.Date >= startDate.Date && r.RevenueDate.Value.Date <= endDate.Date)
                .ToListAsync();
        }
    }
}
