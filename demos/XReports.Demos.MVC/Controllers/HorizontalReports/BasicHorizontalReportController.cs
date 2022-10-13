using System.Collections.Generic;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using XReports.Demos.MVC.Models.Shared;
using XReports.Demos.MVC.XReports;
using XReports.Enums;
using XReports.EpplusFormatters;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;
using XReports.PropertyHandlers;
using XReports.PropertyHandlers.Excel;
using XReports.PropertyHandlers.Html;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Writers;

namespace XReports.Demos.MVC.Controllers.HorizontalReports
{
    public class BasicHorizontalReportController : Controller
    {
        private const int RecordsCount = 10;

        public IActionResult Index()
        {
            IReportTable<ReportCell> reportTable = this.BuildReport();
            IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
            string tableHtml = this.WriteReportToString(htmlReportTable);

            return this.View(new ReportViewModel() { ReportTableHtml = tableHtml });
        }

        public IActionResult Download()
        {
            IReportTable<ReportCell> reportTable = this.BuildReport();
            IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

            Stream excelStream = this.WriteExcelReportToStream(excelReportTable);
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Horizontal.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            EpplusWriter writer = new EpplusWriter(new[]
            {
                new ExcelIndentationPropertyFormatter(),
            });
            return writer.WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            ReportCellProperty centerAlignment = new AlignmentProperty(Alignment.Center);
            BoldProperty bold = new BoldProperty();
            IndentationProperty indentation = new IndentationProperty();

            HorizontalReportSchemaBuilder<Entity> reportBuilder = new HorizontalReportSchemaBuilder<Entity>();

            reportBuilder.AddGlobalProperties(centerAlignment);

            reportBuilder
                .AddHeaderRow(string.Empty, e => e.Name)
                .AddProperties(centerAlignment);
            reportBuilder.AddRow("Age", e => e.Age)
                .AddHeaderProperties(bold);
            reportBuilder.AddRow(new EmptyCellsProvider<Entity>("Score"))
                .AddHeaderProperties(bold);
            reportBuilder.AddRow("Min. Score", e => e.MinScore)
                .AddHeaderProperties(indentation);
            reportBuilder.AddRow("Max. Score", e => e.MaxScore)
                .AddHeaderProperties(indentation);
            reportBuilder.AddRow("Avg. Score", e => e.AverageScore)
                .AddHeaderProperties(indentation)
                .AddProperties(new DecimalPrecisionProperty(2));

            return reportBuilder.BuildSchema().BuildReportTable(this.GetData());
        }

        private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> htmlConverter = new ReportConverter<HtmlReportCell>(new IPropertyHandler<HtmlReportCell>[]
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
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new IPropertyHandler<ExcelReportCell>[]
            {
                new DecimalPrecisionPropertyExcelHandler(),
                new AlignmentPropertyExcelHandler(),
                new BoldPropertyExcelHandler(),
                new ColorPropertyExcelHandler(),
            });

            return excelConverter.Convert(reportTable);
        }

        private string WriteReportToString(IReportTable<HtmlReportCell> htmlReportTable)
        {
            return new BootstrapHtmlStringWriter(new HtmlStringCellWriter()).WriteToString(htmlReportTable);
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

        private class IndentationProperty : ReportCellProperty
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
            protected override void Format(ExcelRange worksheetCell, ExcelReportCell cell, IndentationProperty property)
            {
                worksheetCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheetCell.Style.Indent = property.IndentLevel;
            }
        }
    }
}
