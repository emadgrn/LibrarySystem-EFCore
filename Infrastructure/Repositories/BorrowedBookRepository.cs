using HW12.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Interfaces.Repositories;

namespace HW12.Infrastructure.Repositories
{
    public class BorrowedBookRepository:IBorrowedBookRepository
    {
        private readonly AppDbContext _dbContext = new();

        public int Create(BorrowedBook borrowedBook)
        {
            _dbContext.BorrowedBooks.Add(borrowedBook);
            _dbContext.SaveChanges();

            return borrowedBook.Id;
        }

        public BorrowedBook GetById(int id)
        {
            var borrowedBook = _dbContext.BorrowedBooks.FirstOrDefault(x => x.Id == id);

            if (borrowedBook is null)
                throw new Exception($"Borrowed Book with Id {id} is not found");

            return borrowedBook;
        }

        public List<BorrowedBook> GetAll()
        {
            return _dbContext.BorrowedBooks.ToList();
        }

        public void Update(BorrowedBook borrowedBook)
        {
            var model = GetById(borrowedBook.Id);

            model.BorrowedDate = borrowedBook.BorrowedDate;
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var model = GetById(id);

            _dbContext.BorrowedBooks.Remove(model);
            _dbContext.SaveChanges();
        }
    }
}
