using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.DTO
{
    public class ShowCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ShowBookInCategoryDto> Books { get; set; } = [];
    }

    public class ShowBookInCategoryDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
    }

}
