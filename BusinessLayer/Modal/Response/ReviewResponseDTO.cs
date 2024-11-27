using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Modal.Response
{
    public class ReviewDTO
    {
        public decimal? Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
