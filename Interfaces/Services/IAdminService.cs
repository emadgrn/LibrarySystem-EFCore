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
        ShowUserDto ShowProfile(int userId);
        List<ShowBookDto> ListOfBooks();
        ShowBookDto ShowCompleteInfoOfBook(int bookId);
        List<ShowCategoryDto> ListOfCategories();
        List<ShowUserDto> ListOfUsers();
        List<ShowReviewDto> ListOfReviews();
        bool ActivateUser(int userId);
        bool DeactivateUser(int userId);
        int CreateCategory(string name);
        int CreateBook(string title, string author, int categoryId);
        bool ApproveReview(int reviewId);
        bool RejectReview(int reviewId);
    }
}
