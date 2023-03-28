using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.ConditionalFormatting;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using XReports.Converter;
using XReports.Demos.Models.Shared;
using XReports.Excel;
using XReports.Excel.Writers;
using XReports.Html;
using XReports.Html.Writers;
using XReports.SchemaBuilders;
using XReports.Table;

namespace XReports.Demos.Controllers.EpplusWriterExtensions;

public class ThreeColorHeatmapConditionalController : Controller
{
    private const int RecordsCount = 10;

    public IActionResult Index()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        string tableHtml = this.WriteHtmlReportToString(htmlReportTable);

        return this.View(new ReportViewModel() { ReportTableHtml = tableHtml });
    }

    public IActionResult Download()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        Stream excelStream = this.WriteExcelReportToStream(excelReportTable);
        return this.File(excelStream, Constants.ContentTypeExcel, "3-color Heatmap Using Conditional Formatting.xlsx");
    }

    private IReportTable<ReportCell> BuildReport()
    {
        ThreeColorHeatmapProperty scoreHeatmapProperty = new(0, Color.Red, 50, Color.Yellow, 100, Color.Lime);
        ThreeColorHeatmapProperty lastScoreHeatmapProperty = new(0, Color.Red, 5, Color.Yellow, 10, Color.Lime);

        ReportSchemaBuilder<Entity> reportBuilder = new();
        reportBuilder.AddColumn("Name", e => e.Name);
        reportBuilder.AddColumn("Last Score", e => e.LastScore)
            .AddProperties(lastScoreHeatmapProperty);
        reportBuilder.AddColumn("Score", e => e.Score)
            .AddProperties(scoreHeatmapProperty);

        IReportTable<ReportCell> reportTable = reportBuilder.BuildVerticalSchema().BuildReportTable(this.GetData());
        return reportTable;
    }

    private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
    {
        ReportConverter<HtmlReportCell> htmlConverter = new(new[]
        {
            new ThreeColorHeatmapPropertyHtmlHandler(),
        });

        return htmlConverter.Convert(reportTable);
    }

    private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
    {
        ReportConverter<ExcelReportCell> excelConverter = new(Array.Empty<IPropertyHandler<ExcelReportCell>>());

        return excelConverter.Convert(reportTable);
    }

    private string WriteHtmlReportToString(IReportTable<HtmlReportCell> htmlReportTable)
    {
        return new HtmlStringWriter(new HtmlStringCellWriter()).WriteToString(htmlReportTable);
    }

    private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
    {
        EpplusWriter writer = new(new[]
        {
            new ConditionalFormattingThreeColorScaleFormatter(),
        });

        return writer.WriteToStream(reportTable);
    }

    private IEnumerable<Entity> GetData()
    {
        return new Faker<Entity>()
            .RuleFor(e => e.Name, f => f.Name.FullName())
            .RuleFor(e => e.LastScore, f => f.Random.Int(1, 10))
            .RuleFor(e => e.Score, f => f.Random.Decimal(0, 100))
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