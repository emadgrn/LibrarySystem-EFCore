using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.DTO
{
    public class ShowReviewDto
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public bool IsApproved { get; set; }
    }

}
