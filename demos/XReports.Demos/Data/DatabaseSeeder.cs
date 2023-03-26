using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace XReports.Demos.Data
{
    public class DatabaseSeeder : BackgroundService
    {
        public static bool SeedFinished { get; private set; }

        private readonly IServiceScopeFactory scopeFactory;

        public DatabaseSeeder(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            using IServiceScope serviceScope = this.scopeFactory.CreateScope();
            AppDbContext appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            await this.SeedAsync(appDbContext);

            SeedFinished = true;
        }

        private async Task SeedAsync(AppDbContext appDbContext)
        {
            await appDbContext.Database.EnsureCreatedAsync();
            await appDbContext.Database.MigrateAsync();

            List<User> users = new Faker<User>()
                .RuleFor(u => u.DateOfBirth, f => f.Person.DateOfBirth)
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.IsActive, f => f.Random.Bool())
                .RuleFor(u => u.CreatedOn, f => f.Date.Recent(1000))
                .Generate(1000);
            appDbContext.Users.AddRange(users);

            await appDbContext.SaveChangesAsync();
        }
    }
}
