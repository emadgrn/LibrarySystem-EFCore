using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.DTO;
using HW12.Entities;
using HW12.Infrastructure.Repositories;
using HW12.Interfaces.Repositories;
using HW12.Interfaces.Services;

namespace HW12.Services
{
    public class AdminService(IUserRepository userRepo, IBorrowedBookRepository borrowedBookRepo, IBookRepository bookRepo, ICategoryRepository categoryRepo):IAdminService
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

        public List<ShowUserDto> ListOfUsers()
        {
            return _userRepo.GetAll()
                .Select(u => new ShowUserDto
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    Username = u.Username,
                    IsActive = u.IsActive,
                    Role = u.Role
                })
                .ToList();
        }

        public bool ActivateUser(int userId)
        {
            var user = _userRepo.GetById(userId);
            
            user.IsActive = true;
            _userRepo.Update(user);
            return true;
        }

        public bool DeactivateUser(int userId)
        {
            var user = _userRepo.GetById(userId);
            
            user.IsActive = false;
            _userRepo.Update(user);
            return true;
        }

        public int CreateCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Category name cannot be empty.");

            var category = new Category
            {
                Name = name
            };

            return _categoryRepo.Create(category);
        }

        public int CreateBook(string title, string author, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Book title cannot be empty.");

            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Book author cannot be empty.");

            
            var category = _categoryRepo.GetById(categoryId);
            if (category == null)
                throw new ArgumentException($"Category with ID {categoryId} does not exist.");

            var book = new Book
            {
                Title = title,
                Author = author,
                CategoryId = categoryId,
                IsBorrowed = false
            };

            return _bookRepo.Create(book);
        }
    }

}
