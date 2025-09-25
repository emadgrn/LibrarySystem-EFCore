using HW12.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Interfaces.Services
{
    public interface IRegularUserService
    {
        List<ShowBookDto> ListOfBooks();
        List<ShowCategoryDto> ListOfCategories();
        List<ShowBorrowedBookDto> ListOfBorrowedBooksByUserId(int userId);
        int BorrowBook(int userId, int bookId);
    }
}
