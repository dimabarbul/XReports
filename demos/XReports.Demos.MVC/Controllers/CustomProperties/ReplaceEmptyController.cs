using System.Collections.Generic;
using System.IO;
using Bogus;
using Bogus.Extensions;
using Microsoft.AspNetCore.Mvc;
using XReports.Demos.MVC.Models.Shared;
using XReports.Demos.MVC.XReports;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.PropertyHandlers;
using XReports.SchemaBuilders;
using XReports.Writers;

namespace XReports.Demos.MVC.Controllers.CustomProperties
{
    public class ReplaceEmptyController : Controller
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
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Replace Empty Cells.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            return new EpplusWriter().WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            ReportSchemaBuilder<Entity> reportBuilder = new ReportSchemaBuilder<Entity>();
            reportBuilder.AddColumn("First Name", e => e.FirstName);
            reportBuilder.AddColumn("Last Name", e => e.LastName);
            reportBuilder.AddColumn("Email", e => e.Email)
                .AddProperties(new ReplaceEmptyProperty("-"));
            reportBuilder.AddColumn("Score", e => e.Score)
                .AddProperties(new ReplaceEmptyProperty("(no score)"));

            IReportTable<ReportCell> reportTable = reportBuilder.BuildVerticalSchema().BuildReportTable(this.GetData());
            return reportTable;
        }

        private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> htmlConverter = new ReportConverter<HtmlReportCell>(new[]
            {
                new CustomFormatPropertyHtmlHandler(),
            });

            return htmlConverter.Convert(reportTable);
        }

        private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new[]
            {
                new CustomFormatPropertyExcelHandler(),
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
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName).OrNull(f))
                .RuleFor(e => e.Score, f => f.Random.Int(1, 10).OrNull(f))
                .Generate(RecordsCount);
        }

        private class Entity
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public int? Score { get; set; }
        }

        private class ReplaceEmptyProperty : ReportCellProperty
        {
            public ReplaceEmptyProperty(string text)
            {
                this.Text = text;
            }

            public string Text { get; }
        }

        private class CustomFormatPropertyHtmlHandler : PropertyHandler<ReplaceEmptyProperty, HtmlReportCell>
        {
            protected override void HandleProperty(ReplaceEmptyProperty property, HtmlReportCell cell)
            {
                if (string.IsNullOrEmpty(cell.GetValue<string>()))
                {
                    cell.SetValue(property.Text);
                }
            }
        }

        private class CustomFormatPropertyExcelHandler : PropertyHandler<ReplaceEmptyProperty, ExcelReportCell>
        {
            protected override void HandleProperty(ReplaceEmptyProperty property, ExcelReportCell cell)
            {
                if (string.IsNullOrEmpty(cell.GetValue<string>()))
                {
                    cell.SetValue(property.Text);
                }
            }
        }
    }
}
