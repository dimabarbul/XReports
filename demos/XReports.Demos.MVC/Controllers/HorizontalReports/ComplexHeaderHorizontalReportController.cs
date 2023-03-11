using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using XReports.Converter;
using XReports.Demos.MVC.Models.Shared;
using XReports.Demos.MVC.XReports;
using XReports.Enums;
using XReports.Extensions;
using XReports.Models;
using XReports.Properties;
using XReports.PropertyHandlers.Excel;
using XReports.PropertyHandlers.Html;
using XReports.SchemaBuilder;
using XReports.Table;
using XReports.Writers;

namespace XReports.Demos.MVC.Controllers.HorizontalReports
{
    public class ComplexHeaderHorizontalReportController : Controller
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
            EpplusWriter writer = new EpplusWriter();
            return writer.WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            ReportCellProperty centerAlignment = new AlignmentProperty(Alignment.Center);
            BoldProperty bold = new BoldProperty();

            ReportSchemaBuilder<Entity> reportBuilder = new ReportSchemaBuilder<Entity>();
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
            ReportConverter<HtmlReportCell> htmlConverter = new ReportConverter<HtmlReportCell>(new IPropertyHandler<HtmlReportCell>[]
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
    }
}
