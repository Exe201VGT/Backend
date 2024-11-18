using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Request
{
    public class SellerRegisterDTO
    {
        public string? ShopName { get; set; }
        public string? ShopAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public decimal? Rating { get; set; }
    }

    public class SellerUpdateDTO
    {
        public string? ShopName { get; set; }
        public string? ShopAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public decimal? Rating { get; set; }
    }
}
