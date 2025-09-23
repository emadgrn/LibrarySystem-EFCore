using HW12.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Interfaces.Repositories
{
    public interface IBookRepository
    {
        int Create(Book book);
        Book GetById(int id);
        List<Book> GetAll();
        void Update(Book book);
        void Delete(int id);
    }
}
