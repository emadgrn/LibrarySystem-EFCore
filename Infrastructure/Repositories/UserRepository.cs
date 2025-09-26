using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Entities;
using HW12.Infrastructure.DataAccess;
using HW12.Interfaces.Repositories;

namespace HW12.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext _dbContext) :IUserRepository
    {
        public int Create(User user)
        {
            _dbContext.Users.Add(user);

            return user.Id;
        }

        public User GetById(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            if (user is null)
                throw new Exception($"User with ID {id} is not found");

            return user;
        }

        public List<User> GetAll()
        {
            return _dbContext.Users.ToList();
        }

        public void Update(User user)
        {
            var model = GetById(user.Id);

            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.IsActive = user.IsActive;
            
        }

        public void Delete(int id)
        {
            var model = GetById(id);

            _dbContext.Users.Remove(model);
           
        }
    }
}
