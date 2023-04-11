using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using XReports.Converter;
using XReports.Demos.Models.Shared;
using XReports.Excel;
using XReports.Excel.PropertyHandlers;
using XReports.Excel.Writers;
using XReports.Html;
using XReports.Html.PropertyHandlers;
using XReports.Html.Writers;
using XReports.ReportCellProperties;
using XReports.SchemaBuilders;
using XReports.Table;

namespace XReports.Demos.Controllers.CustomProperties;

public class CustomHeaderPropertiesController : Controller
{
    private const int RecordsCount = 10;

    public IActionResult Index()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        string tableHtml = this.WriteHtmlReportToString(htmlReportTable);

        return this.View(new ReportViewModel() { ReportTableHtml = tableHtml });
    }

    public IActionResult Download()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        Stream excelStream = this.WriteExcelReportToStream(excelReportTable);
        return this.File(excelStream, Constants.ContentTypeExcel, "Custom Header Properties.xlsx");
    }

    private IReportTable<ReportCell> BuildReport()
    {
        ReportSchemaBuilder<Entity> reportBuilder = new();
        reportBuilder.AddColumn("Name", e => e.Name)
            .AddHeaderProperties(new AlignmentProperty(Alignment.Right));
        reportBuilder.AddColumn("Email", e => e.Email)
            .AddHeaderProperties(new ColorProperty(Color.Blue));
        reportBuilder.AddColumn("Score", e => e.Score)
            .AddHeaderProperties(new AlignmentProperty(Alignment.Center));

        IReportTable<ReportCell> reportTable = reportBuilder.BuildVerticalSchema().BuildReportTable(this.GetData());
        return reportTable;
    }

    private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
    {
        ReportConverter<HtmlReportCell> htmlConverter = new(new IPropertyHandler<HtmlReportCell>[]
        {
            new AlignmentPropertyHtmlHandler(),
            new ColorPropertyHtmlHandler(),
        });

        return htmlConverter.Convert(reportTable);
    }

    private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
    {
        ReportConverter<ExcelReportCell> excelConverter = new(new IPropertyHandler<ExcelReportCell>[]
        {
            new AlignmentPropertyExcelHandler(),
            new ColorPropertyExcelHandler(),
        });

        return excelConverter.Convert(reportTable);
    }

    private string WriteHtmlReportToString(IReportTable<HtmlReportCell> htmlReportTable)
    {
        return new HtmlStringWriter(new HtmlStringCellWriter()).WriteToString(htmlReportTable);
    }

    private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
    {
        return new EpplusWriter().WriteToStream(reportTable);
    }

    private IEnumerable<Entity> GetData()
    {
        return new Faker<Entity>()
            .RuleFor(e => e.Name, f => f.Name.FullName())
            .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Name))
            .RuleFor(e => e.Score, f => f.Random.Decimal(80, 100))
            .Generate(RecordsCount);
    }

    private class Entity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public decimal Score { get; set; }
    }
}