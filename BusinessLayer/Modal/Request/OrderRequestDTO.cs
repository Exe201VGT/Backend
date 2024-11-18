using BusinessLayer.Modal.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Request
{
    public class OrderCreateDTO
    {
        public int? BuyerId { get; set; }
        public int? SellerId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Status { get; set; }
        public List<OrderDetailCreateDTO> Details { get; set; }
    }
}
