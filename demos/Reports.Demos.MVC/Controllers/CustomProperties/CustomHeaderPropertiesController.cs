using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Reports.Demos.MVC.Reports;
using Reports.Enums;
using Reports.Excel.EpplusWriter;
using Reports.Extensions;
using Reports.Extensions.Properties;
using Reports.Extensions.Properties.PropertyHandlers.Excel;
using Reports.Extensions.Properties.PropertyHandlers.Html;
using Reports.Interfaces;
using Reports.Models;
using Reports.SchemaBuilders;

namespace Reports.Demos.MVC.Controllers.CustomProperties
{
    public class CustomHeaderPropertiesController : Controller
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
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Custom Header Properties.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            return new EpplusWriter().WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            VerticalReportSchemaBuilder<Entity> reportBuilder = new VerticalReportSchemaBuilder<Entity>();
            reportBuilder.AddColumn("Name", e => e.Name)
                .AddHeaderProperties(new AlignmentProperty(AlignmentType.Right));
            reportBuilder.AddColumn("Email", e => e.Email)
                .AddHeaderProperties(new ColorProperty(Color.Blue));
            reportBuilder.AddColumn("Score", e => e.Score)
                .AddHeaderProperties(new AlignmentProperty(AlignmentType.Center));

            IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(this.GetData());
            return reportTable;
        }

        private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> htmlConverter = new ReportConverter<HtmlReportCell>(new IPropertyHandler<HtmlReportCell>[]
            {
                new AlignmentPropertyHtmlHandler(),
                new ColorPropertyHtmlHandler(),
            });

            return htmlConverter.Convert(reportTable);
        }

        private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new IPropertyHandler<ExcelReportCell>[]
            {
                new AlignmentPropertyExcelHandler(),
                new ColorPropertyExcelHandler(),
            });

            return excelConverter.Convert(reportTable);
        }

        private async Task<string> WriteReportToString(IReportTable<HtmlReportCell> htmlReportTable)
        {
            return await new BootstrapStringWriter().WriteToStringAsync(htmlReportTable);
        }

        public class ViewModel
        {
            public string ReportTableHtml { get; set; }
        }

        private IEnumerable<Entity> GetData()
        {
            return new Faker<Entity>()
                .RuleFor(e => e.Name, f => f.Name.FullName())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Name))
                .RuleFor(e => e.Score, f => Math.Round(f.Random.Decimal(80, 100), 2))
                .Generate(RecordsCount);
        }

        private class Entity
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public decimal Score { get; set; }
        }
    }
}
