using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Request
{
    public class ProductCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public decimal? Weight { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Stock quantity must be at least 1")]
        public int StockQuantity { get; set; }

        public IFormFile ProductImage { get; set; }
    }

    public class ProductUpdateDTO
    {
        [Required]
        public int ProductId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int? CategoryId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? Price { get; set; }

        public decimal? Weight { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Stock quantity must be at least 1")]
        public int? StockQuantity { get; set; }

        public IFormFile ProductImage { get; set; }

    }
}
