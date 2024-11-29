using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Request
{
    public class AddToCartDTO
    {
        //public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }


}
