using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Request
{
    public class SellerRegisterDTO
    {
        [Required(ErrorMessage = "Shop name is required")]
        [MaxLength(100, ErrorMessage = "Shop name cannot exceed 100 characters")]
        public string? ShopName { get; set; }

        [Required(ErrorMessage = "Shop address is required")]
        [MaxLength(200, ErrorMessage = "Shop address cannot exceed 200 characters")]
        public string? ShopAddress { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string? Email { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public decimal? Rating { get; set; }

        public IFormFile? ShopImage { get; set; } // Hỗ trợ tải hình ảnh
    }


    public class SellerUpdateDTO
    {
        [MaxLength(100, ErrorMessage = "Shop name cannot exceed 100 characters")]
        public string? ShopName { get; set; }

        [MaxLength(200, ErrorMessage = "Shop address cannot exceed 200 characters")]
        public string? ShopAddress { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string? Email { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public decimal? Rating { get; set; }

        public IFormFile? ShopImage { get; set; } // Hỗ trợ cập nhật hình ảnh
    }

}
