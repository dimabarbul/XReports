using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Reports.Demos.MVC.Reports;
using Reports.Enums;
using Reports.Excel.EpplusWriter;
using Reports.Excel.Models;
using Reports.Extensions;
using Reports.Extensions.Properties;
using Reports.Extensions.Properties.Handlers.Excel;
using Reports.Extensions.Properties.Handlers.StandardHtml;
using Reports.Html.Models;
using Reports.Interfaces;
using Reports.Models;
using Reports.PropertyHandlers;
using Reports.ReportCellsProviders;
using Reports.SchemaBuilders;

namespace Reports.Demos.MVC.Controllers.HorizontalReports
{
    public class BasicHorizontalReportController : Controller
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
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Horizontal.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            EpplusWriter writer = new EpplusWriter();
            writer.AddFormatter(new ExcelIndentationPropertyFormatter());
            return writer.WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            ReportCellProperty centerAlignment = new AlignmentProperty(AlignmentType.Center);
            BoldProperty bold = new BoldProperty();
            IndentationProperty indentation = new IndentationProperty();

            HorizontalReportSchemaBuilder<Entity> reportBuilder = new HorizontalReportSchemaBuilder<Entity>();
            reportBuilder
                .AddHeaderRow(0, string.Empty, e => e.Name)
                .AddProperties(centerAlignment)
                .AddRow("Age", e => e.Age)
                .AddHeaderProperties(bold)
                .AddProperties(centerAlignment)
                .AddRow(new NullCellsProvider<Entity>("Score"))
                .AddHeaderProperties(bold)
                .AddRow("Min. Score", e => e.MinScore)
                .AddHeaderProperties(indentation)
                .AddProperties(centerAlignment)
                .AddRow("Max. Score", e => e.MaxScore)
                .AddHeaderProperties(indentation)
                .AddProperties(centerAlignment)
                .AddRow("Avg. Score", e => e.AverageScore)
                .AddHeaderProperties(indentation)
                .AddProperties(new DecimalFormatProperty(2), centerAlignment);

            return reportBuilder.BuildSchema().BuildReportTable(this.GetData());
        }

        private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> htmlConverter = new ReportConverter<HtmlReportCell>(new IPropertyHandler<HtmlReportCell>[]
            {
                new StandardHtmlDecimalFormatPropertyHandler(),
                new StandardHtmlAlignmentPropertyHandler(),
                new StandardHtmlBoldPropertyHandler(),
                new StandardHtmlColorPropertyHandler(),
                new HtmlIndentationPropertyHandler(),
            });

            return htmlConverter.Convert(reportTable);
        }

        private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new IPropertyHandler<ExcelReportCell>[]
            {
                new ExcelDecimalFormatPropertyHandler(),
                new ExcelAlignmentPropertyHandler(),
                new ExcelBoldPropertyHandler(),
                new ExcelColorPropertyHandler(),
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
            public int IndentLevel { get; }

            public IndentationProperty(int indentLevel = 1)
            {
                this.IndentLevel = indentLevel;
            }
        }

        private class HtmlIndentationPropertyHandler : SingleTypePropertyHandler<IndentationProperty, HtmlReportCell>
        {
            protected override void HandleProperty(IndentationProperty property, HtmlReportCell cell)
            {
                cell.Styles.Add("padding-left", $"{2 * property.IndentLevel}em");
            }
        }

        private class ExcelIndentationPropertyFormatter : IEpplusFormatter
        {
            public void Format(ExcelRange worksheetCell, ExcelReportCell cell)
            {
                IndentationProperty property = cell.GetProperty<IndentationProperty>();
                if (property == null)
                {
                    return;
                }

                worksheetCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheetCell.Style.Indent = property.IndentLevel;
            }
        }
    }
}
