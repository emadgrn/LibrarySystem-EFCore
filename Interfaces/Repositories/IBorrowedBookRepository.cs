using HW12.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Interfaces.Repositories
{
    public interface IBorrowedBookRepository
    {
        int Create(BorrowedBook borrowedBook);
        BorrowedBook GetById(int id);
        List<BorrowedBook> GetAll();
        void Update(BorrowedBook borrowedBook);
        void Delete(int id);
    }
}
