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

namespace HW12.Services
{
    public class RegularUserService(IUserRepository userRepo, IBorrowedBookRepository borrowedBookRepo, IBookRepository bookRepo, ICategoryRepository categoryRepo):IRegularUserService
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IBorrowedBookRepository _borrowedBookRepo = borrowedBookRepo;
        private readonly IBookRepository _bookRepo = bookRepo;
        private readonly ICategoryRepository _categoryRepo = categoryRepo;


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

        public List<ShowBorrowedBookDto> ListOfBorrowedBooksByUserId(int userId)
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

        public int BorrowBook(int userId,int bookId)
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

            return borrowedBookId;
        }
    }
}
