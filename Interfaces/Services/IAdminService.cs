using HW12.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Interfaces.Services
{
    public interface IAdminService
    {
        List<ShowBookDto> ListOfBooks();
        List<ShowCategoryDto> ListOfCategories();
        List<ShowUserDto> ListOfUsers();
        bool ActivateUser(int userId);
        bool DeactivateUser(int userId);
        int CreateCategory(string name);
        int CreateBook(string title, string author, int categoryId);
    }
}
