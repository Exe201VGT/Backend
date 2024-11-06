using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Response
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int? BuyerId  { get; set; }
        public int? SellerId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
