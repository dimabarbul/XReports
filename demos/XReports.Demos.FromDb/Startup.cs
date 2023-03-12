using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XReports.Demos.FromDb.Data;
using XReports.Demos.FromDb.Services;
using XReports.Demos.FromDb.XReports;
using XReports.DependencyInjection;
using XReports.Excel;
using XReports.Excel.PropertyHandlers;
using XReports.Html;
using XReports.Html.PropertyHandlers;

namespace XReports.Demos.FromDb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddDbContext<AppDbContext>(o =>
            {
                o.UseSqlite("Data Source=Test.db");
            });

            services.AddScoped<UserService>();
            services.AddScoped<ProductService>();
            services.AddScoped<ReportService>();

            this.UseReports(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            Console.WriteLine("Creating DB");
            Stopwatch sw = Stopwatch.StartNew();
            using IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            // context.Database.OpenConnection();
            // context.Database.EnsureDeleted();
            // context.Database.EnsureCreated();
            context.Database.Migrate();
            Console.WriteLine($"DB created, elapsed {sw.ElapsedMilliseconds}");
            sw.Restart();

            DatabaseSeeder.Seed(context);
            Console.WriteLine($"DB seeded, elapsed {sw.ElapsedMilliseconds}");
        }

        private void UseReports(IServiceCollection services)
        {
            services
                .AddHtmlStringWriter<MyHtmlStringWriter>()
                .AddEpplusWriter<MyExcelWriter>()
                .AddReportConverter<HtmlReportCell>(o =>
                {
                    o.AddByBaseType<IHtmlHandler>()
                        .AddFromAssembly(typeof(AlignmentPropertyHtmlHandler).Assembly);
                })
                .AddReportConverter<ExcelReportCell>(o =>
                {
                    o.AddByBaseType<IExcelHandler>()
                        .AddFromAssembly(typeof(AlignmentPropertyExcelHandler).Assembly);
                })
                .AddAttributeBasedBuilder();
        }
    }
}
