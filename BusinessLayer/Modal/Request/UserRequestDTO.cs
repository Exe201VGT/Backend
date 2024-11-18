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
        public int UserId { get; set; }
        public string Status { get; set; }  // VD: "Active", "Inactive", "Suspended"
    }
    public class UserProfileUpdateDTO
    {
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        //[DataType(DataType.Date)]
        //public DateTime? DateOfBirth { get; set; } // Sử dụng DateTime với DataType.Date
        public string? Gender { get; set; }
    }

}
