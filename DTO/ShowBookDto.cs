using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.DTO
{
    public class ShowBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CategoryName { get; set; }
        public List<ShowReviewInBookDto> ApprovedReviews { get; set; } = [];
        public double AverageRating { get; set; }
    }

    public class ShowReviewInBookDto
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
