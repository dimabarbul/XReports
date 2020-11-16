using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.Demos.FromDb.Data;
using Reports.Demos.FromDb.ReportModels;

namespace Reports.Demos.FromDb.Services
{
    public class UserService
    {
        private readonly AppDbContext dbContext;

        public UserService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<UsersListReport>> GetActiveUsersAsync(int? limit)
        {
            IQueryable<UsersListReport> usersQuery = this.dbContext
                .Users
                .Where(u => u.IsActive)
                .Select(u => new UsersListReport()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    CreatedOn = u.CreatedOn,
                    DateOfBirth = u.DateOfBirth,
                    OrdersCount = u.Orders.Count(),
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
