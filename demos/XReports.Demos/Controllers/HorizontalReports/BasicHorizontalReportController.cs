using System.Collections.Generic;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
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
using XReports.SchemaBuilders.ReportCellProviders;
using XReports.Table;

namespace XReports.Demos.Controllers.HorizontalReports;

public class BasicHorizontalReportController : Controller
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
        return this.File(excelStream, Constants.ContentTypeExcel, "BasicHorizontal.xlsx");
    }

    private IReportTable<ReportCell> BuildReport()
    {
        AlignmentProperty centerAlignment = new(Alignment.Center);
        BoldProperty bold = new();
        IndentationProperty indentation = new();

        ReportSchemaBuilder<Entity> reportBuilder = new();

        reportBuilder.AddGlobalProperties(centerAlignment);

        reportBuilder
            .AddColumn(string.Empty, e => e.Name)
            .AddProperties(centerAlignment);
        reportBuilder.AddColumn("Age", e => e.Age)
            .AddHeaderProperties(bold);
        reportBuilder.AddColumn("Score", new EmptyCellProvider<Entity>())
            .AddHeaderProperties(bold);
        reportBuilder.AddColumn("Min. Score", e => e.MinScore)
            .AddHeaderProperties(indentation);
        reportBuilder.AddColumn("Max. Score", e => e.MaxScore)
            .AddHeaderProperties(indentation);
        reportBuilder.AddColumn("Avg. Score", e => e.AverageScore)
            .AddHeaderProperties(indentation)
            .AddProperties(new DecimalPrecisionProperty(2));

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
            new HtmlIndentationPropertyHandler(),
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
        return new HtmlStringWriter(new HtmlStringCellWriter()).Write(htmlReportTable);
    }

    private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
    {
        EpplusWriter writer = new(new[]
        {
            new ExcelIndentationPropertyFormatter(),
        });
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

    private class IndentationProperty : IReportCellProperty
    {
        public IndentationProperty(int indentLevel = 1)
        {
            this.IndentLevel = indentLevel;
        }

        public int IndentLevel { get; }
    }

    private class HtmlIndentationPropertyHandler : PropertyHandler<IndentationProperty, HtmlReportCell>
    {
        protected override void HandleProperty(IndentationProperty property, HtmlReportCell cell)
        {
            cell.Styles.Add("padding-left", $"{2 * property.IndentLevel}em");
        }
    }

    private class ExcelIndentationPropertyFormatter : EpplusFormatter<IndentationProperty>
    {
        protected override void Format(ExcelRange excelRange, ExcelReportCell cell, IndentationProperty property)
        {
            excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            excelRange.Style.Indent = property.IndentLevel;
        }
    }
}
