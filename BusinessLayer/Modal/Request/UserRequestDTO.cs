using BusinessLayer.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Request
{
    public class UserStatusUpdateDTO
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("Active|Inactive|Blocked", ErrorMessage = "Invalid status value")]
        public string Status { get; set; }
    }
    public class UserProfileUpdateDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        public string? Address { get; set; }

        [ValidDateOfBirth] // Custom validation
        public DateOnly? DateOfBirth { get; set; }

        [RegularExpression("Male|Female|Other", ErrorMessage = "Invalid gender")]
        public string? Gender { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }

}
