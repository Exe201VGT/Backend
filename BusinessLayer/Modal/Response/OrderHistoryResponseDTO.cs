using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Response
{
    public class OrderHistoryDTO
    {
        public int OrderHistoryId { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? OrderId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
