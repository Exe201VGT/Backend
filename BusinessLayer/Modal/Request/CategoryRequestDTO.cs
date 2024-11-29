using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Request
{
    public class CategoryCreateDTO
    {
        public string? CategoryName { get; set; }

        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
    }

    public class CategoryUpdateDTO
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
    }

}
