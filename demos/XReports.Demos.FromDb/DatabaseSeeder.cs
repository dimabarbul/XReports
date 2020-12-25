using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.Extensions;
using XReports.Demos.FromDb.Data;
using XReports.Demos.FromDb.Models;

namespace XReports.Demos.FromDb
{
    public static class DatabaseSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Users.Any())
            {
                return;
            }

            List<Product> products = new Faker<Product>()
                .RuleFor(p => p.Price, f => f.Random.Decimal(1, 100))
                .RuleFor(p => p.Title, f => f.Commerce.Product())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.IsActive, f => f.Random.Bool())
                .Generate(1000);
            db.Products.AddRange(products);

            Faker<OrderLineItem> liFaker = new Faker<OrderLineItem>()
                .RuleFor(li => li.Product, f => f.PickRandom(products))
                .RuleFor(li => li.CreatedOn, f => f.Date.Recent(100))
                .RuleFor(li => li.PriceWhenAdded, (f, li) => li.Product.Price - (f.Random.Bool() ? 0 : f.Random.Decimal(1, li.Product.Price)));

            Faker<Order> oFaker = new Faker<Order>()
                .RuleFor(o => o.CreatedOn, f => f.Date.Recent(100))
                .RuleFor(o => o.LineItems, (f, o) =>
                {
                    liFaker.RuleFor(li => li.Order, () => o);

                    return liFaker.GenerateBetween(1, 5);
                });

            List<User> users = new Faker<User>()
                .RuleFor(u => u.DateOfBirth, f => f.Person.DateOfBirth)
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.IsActive, f => f.Random.Bool())
                .RuleFor(u => u.CreatedOn, f => f.Date.Recent(1000))
                .RuleFor(u => u.Orders, (f, u) =>
                {
                    oFaker.RuleFor(o => o.User, () => u);

                    return oFaker.GenerateBetween(1, 15);
                })
                .Generate(1000);
            db.Users.AddRange(users);

            db.SaveChanges();
        }
    }
}
