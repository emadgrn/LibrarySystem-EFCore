using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Entities;
using HW12.Interfaces.Repositories;

namespace HW12.Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _dbContext = new();

        public int Create(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public User GetById(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            if (user is null)
                throw new Exception($"User with Id {id} is not found");

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
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var model = GetById(id);

            _dbContext.Users.Remove(model);
            _dbContext.SaveChanges();
        }
    }
}
