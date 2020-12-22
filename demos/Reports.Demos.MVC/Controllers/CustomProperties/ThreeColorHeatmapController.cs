using System;
using System.Collections.Generic;
using System.Drawing;
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
    public class ThreeColorHeatmapController : Controller
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
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "3-color Heatmap.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            return new EpplusWriter().WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            ThreeColorHeatmapProperty heatmapProperty = new ThreeColorHeatmapProperty(0, Color.Red, 50, Color.Yellow, 100, Color.Lime);

            VerticalReportSchemaBuilder<Entity> reportBuilder = new VerticalReportSchemaBuilder<Entity>();
            reportBuilder
                .AddColumn("Name", e => e.Name)
                .AddColumn("Score", e => e.Score)
                .AddProperties(heatmapProperty);

            IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(this.GetData());
            return reportTable;
        }

        private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> htmlConverter = new ReportConverter<HtmlReportCell>(new[]
            {
                new ThreeColorHeatmapPropertyHtmlHandler(),
            });

            return htmlConverter.Convert(reportTable);
        }

        private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new []
            {
                new ThreeColorHeatmapPropertyExcelHandler(),
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
            return new Faker<Entity>()
                .RuleFor(e => e.Name, f => f.Name.FullName())
                .RuleFor(e => e.Score, f => Math.Round(f.Random.Decimal(0, 100), 2))
                .Generate(RecordsCount);
        }

        private class Entity
        {
            public string Name { get; set; }
            public decimal Score { get; set; }
        }

        private class ThreeColorHeatmapProperty : ReportCellProperty
        {
            public decimal MinimumValue { get; set; }
            public Color MinimumColor { get; set; }
            public decimal MiddleValue { get; set; }
            public Color MiddleColor { get; set; }
            public decimal MaximumValue { get; set; }
            public Color MaximumColor { get; set; }

            public ThreeColorHeatmapProperty(decimal minimumValue, Color minimumColor, decimal middleValue, Color middleColor, decimal maximumValue, Color maximumColor)
            {
                this.MinimumValue = minimumValue;
                this.MinimumColor = minimumColor;
                this.MiddleValue = middleValue;
                this.MiddleColor = middleColor;
                this.MaximumValue = maximumValue;
                this.MaximumColor = maximumColor;
            }

            public Color GetColorForValue(decimal value)
            {
                if (value < this.MiddleValue)
                {
                    return this.GetColorForValue(value, this.MinimumValue, this.MinimumColor, this.MiddleValue, this.MiddleColor);
                }
                else
                {
                    return this.GetColorForValue(value, this.MiddleValue, this.MiddleColor, this.MaximumValue, this.MaximumColor);
                }
            }

            private Color GetColorForValue(decimal value, decimal min, Color minColor, decimal max, Color maxColor)
            {
                decimal valueDelta = max - min;
                decimal valuePercentage = (value - min) / valueDelta;

                byte cellRed = this.GetProportionalValue(valuePercentage, minColor.R, maxColor.R);
                byte cellGreen = this.GetProportionalValue(valuePercentage, minColor.G, maxColor.G);
                byte cellBlue = this.GetProportionalValue(valuePercentage, minColor.B, maxColor.B);

                return Color.FromArgb(cellRed, cellGreen, cellBlue);
            }

            private byte GetProportionalValue(decimal valuePercentage, byte min, byte max)
            {
                return (byte) (min + valuePercentage * (max - min));
            }
        }

        private class ThreeColorHeatmapPropertyHtmlHandler : PropertyHandler<ThreeColorHeatmapProperty, HtmlReportCell>
        {
            protected override void HandleProperty(ThreeColorHeatmapProperty property, HtmlReportCell cell)
            {
                decimal value = cell.GetValue<decimal>();

                cell.Styles.Add("background-color", ColorTranslator.ToHtml(property.GetColorForValue(value)));
            }
        }

        private class ThreeColorHeatmapPropertyExcelHandler : PropertyHandler<ThreeColorHeatmapProperty, ExcelReportCell>
        {
            protected override void HandleProperty(ThreeColorHeatmapProperty property, ExcelReportCell cell)
            {
                decimal value = cell.GetValue<decimal>();
                cell.BackgroundColor = property.GetColorForValue(value);
            }
        }
    }
}
