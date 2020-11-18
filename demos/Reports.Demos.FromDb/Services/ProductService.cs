using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.Demos.FromDb.Data;
using Reports.Demos.FromDb.ReportModels;

namespace Reports.Demos.FromDb.Services
{
    public class ProductService
    {
        private readonly AppDbContext dbContext;

        public ProductService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductListReport>> GetAllAsync(int? limit)
        {
            IQueryable<ProductListReport> productsQuery = this.dbContext
                .Products
                .AsNoTracking()
                .Select(p => new ProductListReport()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    IsActive = p.IsActive,
                });

            if (limit.HasValue)
            {
                productsQuery = productsQuery.Take(limit.Value);
            }

            return await productsQuery.ToArrayAsync();
        }
    }
}
