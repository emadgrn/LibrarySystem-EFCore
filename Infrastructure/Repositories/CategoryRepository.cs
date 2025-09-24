using HW12.Entities;
using HW12.Infrastructure.DataAccess;
using HW12.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext = new();

        public int Create(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return category.Id;
        }

        public Category GetById(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(x => x.Id == id);

            if (category is null)
                throw new Exception($"Category with Id {id} is not found");

            return category;
        }

        public List<Category> GetAll()
        {
            return _dbContext.Categories.ToList();
        }

        public void Update(Category category)
        {
            var model = GetById(category.Id); 

            model.Name = category.Name;
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var model = GetById(id); 

            _dbContext.Categories.Remove(model);
            _dbContext.SaveChanges();
        }
    }
}
