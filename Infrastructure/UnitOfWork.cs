using HW12.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12.Infrastructure
{
    public class UnitOfWork(AppDbContext _dbContext) 
    {
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
