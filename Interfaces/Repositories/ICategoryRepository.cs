using HW12.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        int Create(Category category);
        Category GetById(int id);
        List<Category> GetAll();
        void Update(Category category);
        void Delete(int id);
    }
}
