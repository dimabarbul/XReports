using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using XReports.Converter;
using XReports.Demos.MVC.Models.Shared;
using XReports.Demos.MVC.XReports;
using XReports.Excel;
using XReports.Excel.PropertyHandlers;
using XReports.Excel.Writers;
using XReports.Extensions;
using XReports.Html;
using XReports.Html.PropertyHandlers;
using XReports.Html.Writers;
using XReports.ReportCellProperties;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

namespace XReports.Demos.MVC.Controllers.CustomProperties
{
    public class HighlightRowController : Controller
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
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Custom format.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            return new EpplusWriter().WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            HighlightCellProcessor highlightCellProcessor = new HighlightCellProcessor();

            ReportSchemaBuilder<Entity> reportBuilder = new ReportSchemaBuilder<Entity>();
            reportBuilder.AddColumn("Name", e => e.Name).AddProcessors(highlightCellProcessor);
            reportBuilder.AddColumn("Score", e => e.Score).AddProcessors(highlightCellProcessor);

            IReportTable<ReportCell> reportTable = reportBuilder.BuildVerticalSchema().BuildReportTable(this.GetData());
            return reportTable;
        }

        private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> htmlConverter = new ReportConverter<HtmlReportCell>(new[]
            {
                new ColorPropertyHtmlHandler(),
            });

            return htmlConverter.Convert(reportTable);
        }

        private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new[]
            {
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
                .RuleFor(e => e.Score, f => f.Random.Int(1, 10))
                .Generate(RecordsCount);
        }

        private class Entity
        {
            public string Name { get; set; }

            public int Score { get; set; }
        }

        private class HighlightCellProcessor : IReportCellProcessor<Entity>
        {
            private static readonly ColorProperty Bad = new ColorProperty(null, Color.Red);
            private static readonly ColorProperty Good = new ColorProperty(null, Color.Lime);

            public void Process(ReportCell cell, Entity entity)
            {
                if (entity.Score < 3)
                {
                    cell.AddProperty(Bad);
                }
                else if (entity.Score >= 9)
                {
                    cell.AddProperty(Good);
                }
            }
        }
    }
}
