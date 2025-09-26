using HW12.Entities;
using HW12.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HW12.Infrastructure.Repositories
{
    public class ReviewRepository(AppDbContext _dbContext) : IReviewRepository
    {
        
        public int Create(Review review)
        {
            _dbContext.Reviews.Add(review);
            

            return review.Id;
        }

        public Review GetById(int id)
        {
            var review = _dbContext.Reviews.FirstOrDefault(x => x.Id == id);

            if (review is null)
                throw new Exception($"Review with ID {id} is not found");

            return review;
        }

        public List<Review> GetAll()
        {
            return _dbContext.Reviews
                .Include(r => r.User)
                .Include(r => r.Book)
                .ToList();
        }

        public void Update(Review review)
        {
            var model = GetById(review.Id);

            model.IsApproved = review.IsApproved;
            model.Comment = review.Comment;
            model.CreatedAt = review.CreatedAt;
            model.Rating = review.Rating;
            
        }

        public void Delete(int id)
        {
            var model = GetById(id);

            _dbContext.Reviews.Remove(model);
            
        }
    }
}
