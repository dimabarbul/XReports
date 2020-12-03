using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reports.Demos.FromDb.Data;
using Reports.Demos.FromDb.Reports;
using Reports.Demos.FromDb.Reports.Properties;
using Reports.Demos.FromDb.Services;
using Reports.Excel.EpplusWriter;
using Reports.Extensions.AttributeBasedBuilder;
using Reports.Extensions.AttributeBasedBuilder.AttributeHandlers;
using Reports.Extensions.AttributeBasedBuilder.Interfaces;
using Reports.Extensions.Properties.AttributeHandlers;
using Reports.Extensions.Properties.PropertyHandlers.Excel;
using Reports.Extensions.Properties.PropertyHandlers.Html;
using Reports.Html.StringWriter;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.Demos.FromDb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
            services.AddScoped<StringWriter, MyStringWriter>();
            services.AddScoped<EpplusWriter, MyExcelWriter>();
            services.AddScoped<ReportConverter<HtmlReportCell>>(
                _ => new ReportConverter<HtmlReportCell>(new IPropertyHandler<HtmlReportCell>[]
                {
                    new AlignmentPropertyHtmlHandler(),
                    new BoldPropertyHtmlHandler(),
                    new ColorPropertyHtmlHandler(),
                    new DecimalFormatPropertyHtmlHandler(),
                    new PercentFormatPropertyHtmlHandler(),
                    new DateTimeFormatPropertyHtmlHandler(),

                    new HtmlYesNoPropertyHandler(),
                })
            );
            services.AddScoped<ReportConverter<ExcelReportCell>>(
                _ => new ReportConverter<ExcelReportCell>(new IPropertyHandler<ExcelReportCell>[]
                {
                    new AlignmentPropertyExcelHandler(),
                    new BoldPropertyExcelHandler(),
                    new ColorPropertyExcelHandler(),
                    new DecimalFormatPropertyExcelHandler(),
                    new PercentFormatPropertyExcelHandler(),
                    new DateTimeFormatPropertyExcelHandler(),

                    new ExcelYesNoPropertyHandler(),
                })
            );
            services.AddScoped<AttributeBasedBuilder>(sp => new AttributeBasedBuilder(
                new IAttributeHandler[]
                {
                    new CommonAttributeHandler(),
                    new CustomPropertyAttributeHandler(),
                }
            ));
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
    }
}
