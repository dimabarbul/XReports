using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XReports.Demos.FromDb.Data;
using XReports.Demos.FromDb.ReportModels;

namespace XReports.Demos.FromDb.Services
{
    public class UserService
    {
        private readonly AppDbContext dbContext;

        public UserService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<UsersListReport>> GetActiveUsersAsync(int? limit = null)
        {
            IQueryable<UsersListReport> usersQuery = this.dbContext
                .Users
                .AsNoTracking()
                .Where(u => u.IsActive)
                .Select(u => new UsersListReport()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Age = (int)(DateTime.Now - u.DateOfBirth).TotalDays / 365,
                    RegisteredInDays = (int)(DateTime.Now - u.CreatedOn).TotalDays,
                    OrdersCount = u.Orders.Count,
                });

            if (limit.HasValue)
            {
                usersQuery = usersQuery.Take(limit.Value);
            }

            return await usersQuery
                .ToArrayAsync();
        }
    }
}
