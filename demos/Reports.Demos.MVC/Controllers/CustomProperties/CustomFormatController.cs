using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Reports.Core;
using Reports.Core.Extensions;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Core.PropertyHandlers;
using Reports.Core.SchemaBuilders;
using Reports.Demos.MVC.Reports;
using Reports.Excel.EpplusWriter;
using Reports.Html.StringWriter;

namespace Reports.Demos.MVC.Controllers.CustomProperties
{
    public class CustomFormatController : Controller
    {
        private const int RecordsCount = 10;

        public async Task<IActionResult> Index()
        {
            IReportTable<ReportCell> reportTable = this.BuildReport();
            IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
            string tableHtml = await this.WriteReportToString(htmlReportTable);

            return this.View(new ViewModel() { ReportTableHtml = tableHtml });
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
            VerticalReportSchemaBuilder<Entity> reportBuilder = new VerticalReportSchemaBuilder<Entity>();
            reportBuilder.AddColumn("Name", e => e.Name);
            reportBuilder.AddColumn("Score", e => e.Score)
                .AddProperties(new CustomFormatProperty());

            IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(this.GetData());
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
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new []
            {
                new CustomFormatPropertyExcelHandler(),
            });

            return excelConverter.Convert(reportTable);
        }

        private async Task<string> WriteReportToString(IReportTable<HtmlReportCell> htmlReportTable)
        {
            return await new BootstrapStringWriter(new StringCellWriter()).WriteToStringAsync(htmlReportTable);
        }

        public class ViewModel
        {
            public string ReportTableHtml { get; set; }
        }

        private IEnumerable<Entity> GetData()
        {
            int luckyGuyIndex = new Random().Next(3, RecordsCount - 1);

            return new Faker<Entity>()
                .RuleFor(e => e.Name, f => f.Name.FullName())
                .RuleFor(e => e.Score, f => f.IndexFaker % luckyGuyIndex == 0 ? 100m : f.Random.Decimal(80, 100))
                .Generate(RecordsCount);
        }

        private class Entity
        {
            public string Name { get; set; }
            public decimal Score { get; set; }
        }

        private class CustomFormatProperty : ReportCellProperty { }

        private class CustomFormatPropertyHtmlHandler : PropertyHandler<CustomFormatProperty, HtmlReportCell>
        {
            protected override void HandleProperty(CustomFormatProperty property, HtmlReportCell cell)
            {
                decimal value = cell.GetValue<decimal>();
                string format = value == 100m ? "F0" : "F2";

                cell.Html = value.ToString(format);
            }
        }

        private class CustomFormatPropertyExcelHandler : PropertyHandler<CustomFormatProperty, ExcelReportCell>
        {
            protected override void HandleProperty(CustomFormatProperty property, ExcelReportCell cell)
            {
                cell.NumberFormat = "[=100]0;[<100]0.00";
            }
        }
    }
}
