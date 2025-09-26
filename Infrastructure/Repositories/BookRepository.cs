using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Entities;
using HW12.Infrastructure.DataAccess;
using HW12.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HW12.Infrastructure.Repositories
{
    public class BookRepository(AppDbContext _dbContext) : IBookRepository
    {

        public int Create(Book book)
        {
            _dbContext.Books.Add(book);
            

            return book.Id;
        }

        public Book GetById(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(x => x.Id == id);

            if (book is null)
                throw new Exception($"Book with ID {id} is not found");

            return book;
        }

        public List<Book> GetAll()
        {
            return _dbContext.Books
                .Include(b=>b.Category)
                .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
                .ToList();
        }

        public void Update(Book book)
        {
            var model = GetById(book.Id);

            model.Title = book.Title;
            model.Author = book.Author;
            model.IsBorrowed = book.IsBorrowed;
            
        }

        public void Delete(int id)
        {
            var model = GetById(id);

            _dbContext.Books.Remove(model);
        }
    }

}
