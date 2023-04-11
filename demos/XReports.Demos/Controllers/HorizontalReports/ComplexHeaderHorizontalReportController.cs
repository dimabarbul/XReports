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

namespace XReports.Demos.Controllers.HorizontalReports;

public class ComplexHeaderHorizontalReportController : Controller
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
        return this.File(excelStream, Constants.ContentTypeExcel, "ComplexHeaderHorizontal.xlsx");
    }

    private IReportTable<ReportCell> BuildReport()
    {
        ReportCellProperty centerAlignment = new AlignmentProperty(Alignment.Center);
        BoldProperty bold = new();

        ReportSchemaBuilder<Entity> reportBuilder = new();
        reportBuilder.AddGlobalProperties(centerAlignment);
        reportBuilder
            .AddColumn("Metrics", e => e.Name)
            .AddProperties(centerAlignment)
            .AddHeaderProperties(centerAlignment);
        reportBuilder.AddColumn("Age", e => e.Age)
            .AddHeaderProperties(bold);
        reportBuilder.AddColumn("Min. Score", e => e.MinScore);
        reportBuilder.AddColumn("Max. Score", e => e.MaxScore);
        reportBuilder.AddColumn("Avg. Score", e => e.AverageScore)
            .AddProperties(new DecimalPrecisionProperty(2));

        reportBuilder.AddComplexHeader(0, "Score", "Min. Score", "Avg. Score");
        reportBuilder.AddComplexHeaderProperties("Score", new ColorProperty(Color.Blue));

        return reportBuilder.BuildHorizontalSchema(1).BuildReportTable(this.GetData());
    }

    private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
    {
        ReportConverter<HtmlReportCell> htmlConverter = new(new IPropertyHandler<HtmlReportCell>[]
        {
            new DecimalPrecisionPropertyHtmlHandler(),
            new AlignmentPropertyHtmlHandler(),
            new BoldPropertyHtmlHandler(),
            new ColorPropertyHtmlHandler(),
        });

        return htmlConverter.Convert(reportTable);
    }

    private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
    {
        ReportConverter<ExcelReportCell> excelConverter = new(new IPropertyHandler<ExcelReportCell>[]
        {
            new DecimalPrecisionPropertyExcelHandler(),
            new AlignmentPropertyExcelHandler(),
            new BoldPropertyExcelHandler(),
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
        EpplusWriter writer = new();
        return writer.WriteToStream(reportTable);
    }

    private IEnumerable<Entity> GetData()
    {
        return new Faker<Entity>()
            .RuleFor(e => e.Name, f => f.Name.FullName())
            .RuleFor(e => e.MinScore, f => f.Random.Int(1, 10))
            .RuleFor(e => e.MaxScore, (f, e) => f.Random.Int(e.MinScore, 10))
            .RuleFor(e => e.AverageScore, (f, e) => f.Random.Decimal(e.MinScore, e.MaxScore))
            .RuleFor(e => e.Age, f => f.Random.Int(18, 63))
            .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Name))
            .Generate(RecordsCount);
    }

    private class Entity
    {
        public string Name { get; set; }

        public decimal AverageScore { get; set; }

        public int MinScore { get; set; }

        public int MaxScore { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }
    }
}