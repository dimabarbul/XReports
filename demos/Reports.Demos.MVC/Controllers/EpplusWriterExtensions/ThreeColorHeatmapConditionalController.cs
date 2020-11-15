using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.ConditionalFormatting;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using Reports.Builders;
using Reports.Demos.MVC.Reports;
using Reports.Excel.EpplusWriter;
using Reports.Excel.Models;
using Reports.Extensions;
using Reports.Html.Models;
using Reports.Interfaces;
using Reports.Models;
using Reports.PropertyHandlers;

namespace Reports.Demos.MVC.Controllers.EpplusWriterExtensions
{
    public class ThreeColorHeatmapConditionalController : Controller
    {
        private const int RecordsCount = 20;

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
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "3-color Heatmap Using Conditional Formatting.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            Excel.EpplusWriter.EpplusWriter writer = new Excel.EpplusWriter.EpplusWriter();
            writer.AddFormatter(new ConditionalFormattingThreeColorScaleFormatter());

            return writer.WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            ThreeColorHeatmapProperty scoreHeatmapProperty = new ThreeColorHeatmapProperty(0, Color.Red, 50, Color.Yellow, 100, Color.Lime);
            ThreeColorHeatmapProperty lastScoreHeatmapProperty = new ThreeColorHeatmapProperty(0, Color.Red, 5, Color.Yellow, 10, Color.Lime);

            VerticalReportBuilder<Entity> reportBuilder = new VerticalReportBuilder<Entity>();
            reportBuilder.AddColumn("Name", e => e.Name);
            reportBuilder.AddColumn("Last Score", e => e.LastScore)
                .AddProperty(lastScoreHeatmapProperty);
            reportBuilder.AddColumn("Score", e => e.Score)
                .AddProperty(scoreHeatmapProperty);

            IReportTable<ReportCell> reportTable = reportBuilder.Build(this.GetData());
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
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new IPropertyHandler<ExcelReportCell>[]
            {
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
                .RuleFor(e => e.LastScore, f => f.Random.Int(1, 10))
                .RuleFor(e => e.Score, f => Math.Round(f.Random.Decimal(0, 100), 2))
                .Generate(RecordsCount);
        }

        private class Entity
        {
            public string Name { get; set; }
            public int LastScore { get; set; }
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

        private class ThreeColorHeatmapPropertyHtmlHandler : SingleTypePropertyHandler<ThreeColorHeatmapProperty, HtmlReportCell>
        {
            protected override void HandleProperty(ThreeColorHeatmapProperty property, HtmlReportCell cell)
            {
                decimal value = cell.GetValue<decimal>();

                cell.Styles.Add("background-color", ColorTranslator.ToHtml(property.GetColorForValue(value)));
            }
        }

        // As there is no handler for the property, it ends up in Properties collection of Excel report cell.
        private class ConditionalFormattingThreeColorScaleFormatter : IEpplusFormatter
        {
            public void Format(ExcelRange worksheetCell, ExcelReportCell cell)
            {
                ThreeColorHeatmapProperty property = cell.GetProperty<ThreeColorHeatmapProperty>();

                // continue only if property is set on cell
                if (property == null)
                {
                    return;
                }

                IExcelConditionalFormattingThreeColorScale threeColorScale = worksheetCell.ConditionalFormatting.AddThreeColorScale();
                threeColorScale.LowValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                threeColorScale.LowValue.Value = (double) property.MinimumValue;
                threeColorScale.LowValue.Color = property.MinimumColor;
                threeColorScale.MiddleValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                threeColorScale.MiddleValue.Value = (double) property.MiddleValue;
                threeColorScale.MiddleValue.Color = property.MiddleColor;
                threeColorScale.HighValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                threeColorScale.HighValue.Value = (double) property.MaximumValue;
                threeColorScale.HighValue.Color = property.MaximumColor;
            }
        }
    }
}
