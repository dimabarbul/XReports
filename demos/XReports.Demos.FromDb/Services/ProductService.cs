using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XReports.Demos.FromDb.Data;
using XReports.Demos.FromDb.ReportModels;

namespace XReports.Demos.FromDb.Services
{
    public class ProductService
    {
        private readonly AppDbContext dbContext;

        public ProductService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductListReport>> GetAllAsync(int? limit = null)
        {
            IQueryable<ProductListReport> productsQuery = this.dbContext
                .Products
                .AsNoTracking()
                .Select(p => new ProductListReport()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.IsActive ? p.Price : (decimal?) null,
                    IsActive = p.IsActive,
                });

            if (limit.HasValue)
            {
                productsQuery = productsQuery.Take(limit.Value);
            }

            return await productsQuery.ToArrayAsync();
        }

        public async Task<IEnumerable<OrdersDetailsReport>> GetOrdersDetailsAsync(int? limit = null)
        {
            IQueryable<OrdersDetailsReport> detailsQuery = this.dbContext
                .OrderLineItems
                .AsNoTracking()
                .Select(li => new OrdersDetailsReport()
                {
                    LineItemId = li.Id,
                    OrderId = li.OrderId,
                    CreatedOn = li.CreatedOn,
                    PriceWhenAdded = li.PriceWhenAdded,
                    ProductId = li.ProductId,
                    ProductTitle = li.Product.Title,
                    ProductDescription = li.Product.Description,
                    ProductPrice = li.Product.Price,
                    ProductIsActive = li.Product.IsActive,
                    UserFirstName = li.Order.User.FirstName,
                    UserLastName = li.Order.User.LastName,
                    UserEmail = li.Order.User.Email,
                    UserDateOfBirth = li.Order.User.DateOfBirth,
                    UserCreatedOn = li.Order.User.CreatedOn,
                    UserIsActive = li.Order.User.IsActive,
                });

            detailsQuery = detailsQuery.Take(limit ?? 100000);

            return await detailsQuery.ToArrayAsync();
        }
    }
}
