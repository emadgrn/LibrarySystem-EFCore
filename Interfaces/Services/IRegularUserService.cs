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
        ShowUserDto ShowProfile(int userId);
        List<ShowBookDto> ListOfBooks();
        ShowBookDto ShowCompleteInfoOfBook(int bookId);
        List<ShowCategoryDto> ListOfCategories();
        List<ShowBorrowedBookDto> ListOfBorrowedBooksByUser(int userId);
        List<ShowReviewDto> ListOfReviewsByUser(int userId);
        int BorrowBook(int userId, int bookId);
        int CreateReview(int userId, int bookId, int rating, string comment);
        bool EditReviewComment(int userId, int reviewId, string comment);
        bool DeleteReview(int userId, int reviewId);

    }
}
