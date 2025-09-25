using HW12.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        int Create(Review review);
        Review GetById(int id);
        List<Review> GetAll();
        void Update(Review review);
        void Delete(int id);
    }
}
