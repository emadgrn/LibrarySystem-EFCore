using HW12.DTO;
using HW12.Entities;
using HW12.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Interfaces.Services;
using HW12.Infrastructure;

namespace HW12.Services
{
    public class RegularUserService(IUserRepository _userRepo, IBorrowedBookRepository _borrowedBookRepo, IBookRepository _bookRepo, ICategoryRepository _categoryRepo, IReviewRepository _reviewRepo, UnitOfWork _unitOfWork) : IRegularUserService
    {
        public ShowUserDto ShowProfile(int userId)
        {
            var user= _userRepo.GetById(userId);

            return new ShowUserDto
            {
                Id = user.Id,
                FullName = user.FirstName + " " + user.LastName,
                Username = user.Username,
                IsActive = user.IsActive,
                Role = user.Role
            };
        }
        public List<ShowBookDto> ListOfBooks()
        {
            return _bookRepo.GetAll()
                .Select(b => new ShowBookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    CategoryName = b.Category.Name
                })
                .ToList();
        }
        public ShowBookDto ShowCompleteInfoOfBook(int bookId)
        {
            var book=_bookRepo.GetById(bookId);

            var approvedReviews = book.Reviews
                .Where(r => r.IsApproved)
                .Select(r => new ShowReviewInBookDto
                {
                    UserId = r.UserId,
                    UserFullName = r.User.FirstName + " " + r.User.LastName,
                    Comment = r.Comment,
                    Rating = r.Rating
                })
                .ToList();

            double avgRating = approvedReviews.Any()
                ? approvedReviews.Average(r => r.Rating)
                : 0;

            var dto = new ShowBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryName = book.Category.Name,
                ApprovedReviews = approvedReviews,
                AverageRating = avgRating
            };

            return dto;
        }
        public List<ShowCategoryDto> ListOfCategories()
        {
            return _categoryRepo.GetAll()
                .Select(c => new ShowCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Books = c.Books.Select(b => new ShowBookInCategoryDto
                    {
                        Title = b.Title,
                        Author = b.Author
                    }).ToList()
                })
                .ToList();
        }
        public List<ShowBorrowedBookDto> ListOfBorrowedBooksByUser(int userId)
        {
            return _borrowedBookRepo.GetAll()
                .Where(bb => bb.UserId == userId)
                .Select(bb => new ShowBorrowedBookDto
                {
                    BorrowedBookId = bb.Id,
                    BorrowingDateTime = bb.BorrowingDateTime,
                    UserId = bb.UserId,
                    UserFullName = bb.User.FirstName + " " + bb.User.LastName,
                    BookId = bb.BookId,
                    BookTitle = bb.Book.Title,
                    CategoryName = bb.Book.Category.Name
                })
                .ToList();
        }
        public List<ShowReviewDto> ListOfReviewsByUser(int userId)
        {
            return _reviewRepo.GetAll()
                .Where(r => r.UserId == userId)
                .Select(r => new ShowReviewDto
                {
                    ReviewId = r.Id,
                    UserId = r.UserId,
                    UserFullName = r.User.FirstName + " " + r.User.LastName,
                    BookId = r.BookId,
                    BookTitle = r.Book.Title,
                    Comment = r.Comment,
                    Rating = r.Rating,
                    IsApproved = r.IsApproved
                })
                .ToList();
        }
        public int BorrowBook(int userId, int bookId)
        {
            var book = _bookRepo.GetById(bookId);

            if (book.IsBorrowed)
                throw new Exception($"Book with ID {bookId} is already borrowed.");

            var borrowedBook = new BorrowedBook
            {
                UserId = userId,
                BookId = bookId,
                BorrowingDateTime = DateTime.Now
            };

            int borrowedBookId = _borrowedBookRepo.Create(borrowedBook);

            book.IsBorrowed = true;
            _bookRepo.Update(book);
            _unitOfWork.Save();

            return borrowedBookId;
        }
        public int CreateReview(int userId, int bookId, int rating, string comment)
        {
            var book = _bookRepo.GetById(bookId);

            if (rating < 1 || rating > 5)
                throw new Exception("Rating must be between 1 and 5.");

            var review = new Review
            {
                UserId = userId,
                BookId = bookId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now,
                IsApproved = false
            };

            int reviewId=_reviewRepo.Create(review);
            _unitOfWork.Save();

            return reviewId;
        }
        public bool EditReviewComment(int userId, int reviewId, string comment)
        {
            var review = _reviewRepo.GetById(reviewId);

            if (review.UserId != userId)
                throw new Exception("You can only edit your own reviews!");

            review.Comment = comment;
            _reviewRepo.Update(review);
            _unitOfWork.Save();


            return true;
        }
        public bool DeleteReview(int userId, int reviewId)
        {
            var review = _reviewRepo.GetById(reviewId);

            if (review.UserId != userId)
                throw new Exception("You can only delete your own reviews!");

            _reviewRepo.Delete(reviewId);
            _unitOfWork.Save();

            return true;
        }
    }
}
