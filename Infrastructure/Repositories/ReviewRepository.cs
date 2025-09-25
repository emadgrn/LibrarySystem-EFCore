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
    public class ReviewRepository:IReviewRepository
    {
        private readonly AppDbContext _dbContext = new();

        public int Create(Review review)
        {
            _dbContext.Reviews.Add(review);
            _dbContext.SaveChanges();

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
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var model = GetById(id);

            _dbContext.Reviews.Remove(model);
            _dbContext.SaveChanges();
        }
    }
}
