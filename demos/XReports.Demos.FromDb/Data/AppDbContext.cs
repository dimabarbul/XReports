using Microsoft.EntityFrameworkCore;
using XReports.Demos.FromDb.Models;

namespace XReports.Demos.FromDb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderLineItem> OrderLineItems { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasMany(o => o.LineItems)
                .WithOne(li => li.Order)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<OrderLineItem>()
                .HasOne(li => li.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
