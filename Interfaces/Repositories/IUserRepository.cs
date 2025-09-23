using HW12.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Interfaces.Repositories
{
    public interface IUserRepository
    {
        int Create(User user);
        User GetById(int id);
        List<User> GetAll();
        void Update(User user);
        void Delete(int id);
    }
}
