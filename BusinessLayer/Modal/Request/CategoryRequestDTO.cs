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

        public string? Image { get; set; }
    }

    public class CategoryUpdateDTO
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }
    }

}
