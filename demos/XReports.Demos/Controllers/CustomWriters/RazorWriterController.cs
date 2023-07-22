using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using XReports.Converter;
using XReports.Demos.Models.Shared;
using XReports.Demos.XReports;
using XReports.Html;
using XReports.Html.PropertyHandlers;
using XReports.ReportCellProperties;
using XReports.SchemaBuilders;
using XReports.Table;

namespace XReports.Demos.Controllers.CustomWriters;

public class RazorWriterController : Controller
{
    private const int RecordsCount = 10;

    private readonly IServiceProvider serviceProvider;

    public RazorWriterController(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<IActionResult> Index()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        string tableHtml = await this.WriteHtmlReportToStringAsync(htmlReportTable);

        return this.View(new ReportViewModel() { ReportTableHtml = tableHtml });
    }

    private IReportTable<ReportCell> BuildReport()
    {
        ReportSchemaBuilder<Entity> reportBuilder = new();
        reportBuilder.AddColumn("Name", e => e.Name)
            .AddProperties(new BoldProperty());
        reportBuilder.AddColumn("Score", e => e.Score);

        IReportTable<ReportCell> reportTable = reportBuilder.BuildVerticalSchema().BuildReportTable(this.GetData());
        return reportTable;
    }

    private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
    {
        ReportConverter<HtmlReportCell> htmlConverter = new(new[]
        {
            new BoldPropertyHtmlHandler(),
        });

        return htmlConverter.Convert(reportTable);
    }

    private Task<string> WriteHtmlReportToStringAsync(IReportTable<HtmlReportCell> htmlReportTable)
    {
        return new RazorWriter(this.serviceProvider)
            .WriteAsync(htmlReportTable, "~/Views/RazorWriter/Report.cshtml");
    }

    private IEnumerable<Entity> GetData()
    {
        return new Faker<Entity>()
            .RuleFor(e => e.Name, f => f.Name.FullName())
            .RuleFor(e => e.Score, f => f.Random.Decimal(0, 100))
            .Generate(RecordsCount);
    }

    private class Entity
    {
        public string Name { get; set; }

        public decimal Score { get; set; }
    }
}
