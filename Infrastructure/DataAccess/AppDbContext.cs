using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW12.Entities;

namespace HW12.Infrastructure.DataAccess
{
    public class AppDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    "Server=.\\SQLEXPRESS;Database=Library-EFCore;Trusted_Connection=True;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Review>(entity =>
            {
                // Primary Key

                entity.HasKey(r => r.Id);


                // Properties

                entity.Property(r => r.Comment)
                    .HasMaxLength(500) 
                    .IsRequired(false); 

                entity.Property(r => r.CreatedAt)
                    .IsRequired();

                entity.Property(r => r.IsApproved)
                    .HasDefaultValue(false)   
                    .IsRequired();

                entity.Property(r => r.Rating)
                    .IsRequired();

                //Constraints
                entity.HasCheckConstraint("CK_Review_Rating", "[Rating] >= 1 AND [Rating] <= 5");


                // Relationships

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Reviews) 
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne(r => r.Book)
                    .WithMany(b => b.Reviews) 
                    .HasForeignKey(r => r.BookId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });
        }

    }
}
