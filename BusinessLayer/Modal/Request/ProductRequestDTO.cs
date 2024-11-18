using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Request
{
    public class ProductCreateDTO
    {
        public int? SellerId { get; set; }
        public string? Name { get; set; }

        public int? CategoryId { get; set; }

        public decimal? Price { get; set; }

        public decimal? Weight { get; set; }

        public string? Description { get; set; }

        public int? StockQuantity { get; set; }
    }

    public class ProductUpdateDTO
    {
        public int ProductId { get; set; }
        public int? SellerId { get; set; }
        public string? Name { get; set; }

        public int? CategoryId { get; set; }

        public decimal? Price { get; set; }

        public decimal? Weight { get; set; }

        public string? Description { get; set; }

        public int? StockQuantity { get; set; }

    }
}
