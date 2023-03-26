using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using XReports.Demos.MVC.Data;
using XReports.Demos.MVC.XReports;
using XReports.DependencyInjection;
using XReports.Excel;
using XReports.Excel.PropertyHandlers;
using XReports.Html;
using XReports.Html.PropertyHandlers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlite("Data Source=file::memory:?cache=shared");
});

AddXReports(builder.Services);

builder.Services.AddHostedService<DatabaseSeeder>();

WebApplication app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();

static void AddXReports(IServiceCollection services)
{
    services
        // custom implementation for IHtmlStringWriter
        .AddHtmlStringWriter<BootstrapHtmlStringWriter>()
        // default implementation for IEpplusWriter
        .AddEpplusWriter()
        // will be available by injecting IReportConverterFactory<HtmlReportCell>
        // and calling Get("no-handlers")
        .AddReportConverter<HtmlReportCell>("no-handlers")
        // will be available by injecting IReportConverterFactory<ExcelReportCell>
        // and calling Get("no-handlers")
        .AddReportConverter<ExcelReportCell>("no-handlers")
        // default converter will be available by injecting IReportConverter<HtmlReportCell>
        .AddReportConverter<HtmlReportCell>(o =>
        {
            o.AddFromAssembly(typeof(AlignmentPropertyHtmlHandler).Assembly);
        })
        // default converter will be available by injecting IReportConverter<ExcelReportCell>
        .AddReportConverter<ExcelReportCell>(o =>
        {
            o.AddFromAssembly(typeof(AlignmentPropertyExcelHandler).Assembly);
        })
        .AddAttributeBasedBuilder(o =>
        {
            o.AddFromAssembly(typeof(XReports.SchemaBuilders.Attributes.AlignmentAttribute).Assembly);
        });
}
