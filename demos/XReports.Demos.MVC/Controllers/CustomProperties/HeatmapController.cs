using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using XReports.Demos.MVC.XReports;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.PropertyHandlers;
using XReports.SchemaBuilders;
using XReports.Writers;

namespace XReports.Demos.MVC.Controllers.CustomProperties
{
    public class HeatmapController : Controller
    {
        private const int RecordsCount = 50;

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
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Heatmap.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            return new EpplusWriter().WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            HeatmapProperty heatmapProperty = new HeatmapProperty(0, Color.IndianRed, 100, Color.SkyBlue);

            VerticalReportSchemaBuilder<Entity> reportBuilder = new VerticalReportSchemaBuilder<Entity>();
            reportBuilder.AddColumn("Name", e => e.Name);
            reportBuilder.AddColumn("Score", e => e.Score)
                .AddProperties(heatmapProperty);

            IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(this.GetData());
            return reportTable;
        }

        private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> htmlConverter = new ReportConverter<HtmlReportCell>(new[]
            {
                new HeatmapPropertyHtmlHandler(),
            });

            return htmlConverter.Convert(reportTable);
        }

        private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new[]
            {
                new HeatmapPropertyExcelHandler(),
            });

            return excelConverter.Convert(reportTable);
        }

        private async Task<string> WriteReportToString(IReportTable<HtmlReportCell> htmlReportTable)
        {
            return await new BootstrapStringWriter(new StringCellWriter()).WriteToStringAsync(htmlReportTable);
        }

        private IEnumerable<Entity> GetData()
        {
            return new Faker<Entity>()
                .RuleFor(e => e.Name, f => f.Name.FullName())
                .RuleFor(e => e.Score, f => Math.Round(f.Random.Decimal(0, 100), 2))
                .Generate(RecordsCount);
        }

        public class ViewModel
        {
            public string ReportTableHtml { get; set; }
        }

        private class Entity
        {
            public string Name { get; set; }

            public decimal Score { get; set; }
        }

        private class HeatmapProperty : ReportCellProperty
        {
            private readonly decimal minimumValue;
            private readonly Color minimumColor;
            private readonly decimal maximumValue;
            private readonly Color maximumColor;

            public HeatmapProperty(decimal minimumValue, Color minimumColor, decimal maximumValue, Color maximumColor)
            {
                this.minimumValue = minimumValue;
                this.minimumColor = minimumColor;
                this.maximumValue = maximumValue;
                this.maximumColor = maximumColor;
            }

            public Color GetColorForValue(decimal value)
            {
                decimal heatmapValueDelta = this.maximumValue - this.minimumValue;
                decimal valuePercentage = (value - this.minimumValue) / heatmapValueDelta;

                byte cellRed = this.GetProportionalValue(valuePercentage, this.minimumColor.R, this.maximumColor.R);
                byte cellGreen = this.GetProportionalValue(valuePercentage, this.minimumColor.G, this.maximumColor.G);
                byte cellBlue = this.GetProportionalValue(valuePercentage, this.minimumColor.B, this.maximumColor.B);

                return Color.FromArgb(cellRed, cellGreen, cellBlue);
            }

            private byte GetProportionalValue(decimal valuePercentage, byte min, byte max)
            {
                return (byte)(min + (valuePercentage * (max - min)));
            }
        }

        private class HeatmapPropertyHtmlHandler : PropertyHandler<HeatmapProperty, HtmlReportCell>
        {
            protected override void HandleProperty(HeatmapProperty property, HtmlReportCell cell)
            {
                decimal value = cell.GetValue<decimal>();

                cell.Styles.Add("background-color", ColorTranslator.ToHtml(property.GetColorForValue(value)));
            }
        }

        private class HeatmapPropertyExcelHandler : PropertyHandler<HeatmapProperty, ExcelReportCell>
        {
            protected override void HandleProperty(HeatmapProperty property, ExcelReportCell cell)
            {
                decimal value = cell.GetValue<decimal>();
                cell.BackgroundColor = property.GetColorForValue(value);
            }
        }
    }
}
