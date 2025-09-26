using HW12.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Interfaces.Repositories;
using HW12.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace HW12.Infrastructure.Repositories
{
    public class BorrowedBookRepository(AppDbContext _dbContext) : IBorrowedBookRepository
    {
        

        public int Create(BorrowedBook borrowedBook)
        {
            _dbContext.BorrowedBooks.Add(borrowedBook);
            

            return borrowedBook.Id;
        }

        public BorrowedBook GetById(int id)
        {
            var borrowedBook = _dbContext.BorrowedBooks.FirstOrDefault(x => x.Id == id);

            if (borrowedBook is null)
                throw new Exception($"Borrowed Book with ID {id} is not found");

            return borrowedBook;
        }

        public List<BorrowedBook> GetAll()
        {
            return _dbContext.BorrowedBooks
                .Include(bb => bb.User)
                .Include(bb => bb.Book)
                .ThenInclude(b => b.Category)
                .ToList();
        }

        public void Update(BorrowedBook borrowedBook)
        {
            var model = GetById(borrowedBook.Id);

            model.BorrowingDateTime = borrowedBook.BorrowingDateTime;
           
        }

        public void Delete(int id)
        {
            var model = GetById(id);

            _dbContext.BorrowedBooks.Remove(model);
            
        }
    }
}
