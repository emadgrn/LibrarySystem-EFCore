using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Entities.Enums;

namespace HW12.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public RoleEnum Role { get; set; }
        public List<BorrowedBook> BorrowedBooks { get; set; } = [];
        public List<Review> Reviews { get; set; } = [];

        public bool ConfirmPassword(string password)
        {
            return password == Password;
        }
	}


}
