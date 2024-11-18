using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Response
{
    public class RevenueDTO
    {
        public int RevenueId { get; set; }
        public int? SellerId { get; set; }
        public int? OrderId { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? RevenueDate { get; set; }
        public string? Status { get; set; }

    }
    public class RevenueSummaryDTO
    {
        public decimal TotalRevenue { get; set; }
        public int OrderCount { get; set; }
        public List<RevenueDTO> Details { get; set; }
    }
}
