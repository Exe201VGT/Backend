using BOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Response
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public decimal? Weight { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public decimal? AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public string ProductImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public SellerDTO Seller { get; set; } 
        public List<ReviewDTO> Reviews { get; set; } 

    }
}
