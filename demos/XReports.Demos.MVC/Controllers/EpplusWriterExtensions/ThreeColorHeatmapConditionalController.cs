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
using XReports.Demos.MVC.XReports;
using XReports.EpplusFormatters;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.PropertyHandlers;
using XReports.SchemaBuilders;
using XReports.Writers;

namespace XReports.Demos.MVC.Controllers.EpplusWriterExtensions
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
            EpplusWriter writer = new EpplusWriter();
            writer.AddFormatter(new ConditionalFormattingThreeColorScaleFormatter());

            return writer.WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            ThreeColorHeatmapProperty scoreHeatmapProperty = new ThreeColorHeatmapProperty(0, Color.Red, 50, Color.Yellow, 100, Color.Lime);
            ThreeColorHeatmapProperty lastScoreHeatmapProperty = new ThreeColorHeatmapProperty(0, Color.Red, 5, Color.Yellow, 10, Color.Lime);

            VerticalReportSchemaBuilder<Entity> reportBuilder = new VerticalReportSchemaBuilder<Entity>();
            reportBuilder
                .AddColumn("Name", e => e.Name)
                .AddColumn("Last Score", e => e.LastScore)
                .AddProperties(lastScoreHeatmapProperty);
            reportBuilder.AddColumn("Score", e => e.Score)
                .AddProperties(scoreHeatmapProperty);

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
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new IPropertyHandler<ExcelReportCell>[]
            {
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
                .RuleFor(e => e.LastScore, f => f.Random.Int(1, 10))
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

            public int LastScore { get; set; }

            public decimal Score { get; set; }
        }

        private class ThreeColorHeatmapProperty : ReportCellProperty
        {
            public ThreeColorHeatmapProperty(decimal minimumValue, Color minimumColor, decimal middleValue, Color middleColor, decimal maximumValue, Color maximumColor)
            {
                this.MinimumValue = minimumValue;
                this.MinimumColor = minimumColor;
                this.MiddleValue = middleValue;
                this.MiddleColor = middleColor;
                this.MaximumValue = maximumValue;
                this.MaximumColor = maximumColor;
            }

            public decimal MinimumValue { get; set; }

            public Color MinimumColor { get; set; }

            public decimal MiddleValue { get; set; }

            public Color MiddleColor { get; set; }

            public decimal MaximumValue { get; set; }

            public Color MaximumColor { get; set; }

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
                return (byte)(min + (valuePercentage * (max - min)));
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

        private class ConditionalFormattingThreeColorScaleFormatter : EpplusFormatter<ThreeColorHeatmapProperty>
        {
            protected override void Format(ExcelRange worksheetCell, ExcelReportCell cell, ThreeColorHeatmapProperty property)
            {
                IExcelConditionalFormattingThreeColorScale threeColorScale = worksheetCell.ConditionalFormatting.AddThreeColorScale();
                threeColorScale.LowValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                threeColorScale.LowValue.Value = (double)property.MinimumValue;
                threeColorScale.LowValue.Color = property.MinimumColor;
                threeColorScale.MiddleValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                threeColorScale.MiddleValue.Value = (double)property.MiddleValue;
                threeColorScale.MiddleValue.Color = property.MiddleColor;
                threeColorScale.HighValue.Type = eExcelConditionalFormattingValueObjectType.Num;
                threeColorScale.HighValue.Value = (double)property.MaximumValue;
                threeColorScale.HighValue.Color = property.MaximumColor;
            }
        }
    }
}
