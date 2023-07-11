using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using XReports.Converter;
using XReports.Demos.Models.Shared;
using XReports.Excel;
using XReports.Excel.Writers;
using XReports.Html;
using XReports.Html.Writers;
using XReports.SchemaBuilders;
using XReports.Table;

namespace XReports.Demos.Controllers.CustomProperties;

public class ThreeColorHeatmapController : Controller
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
        return this.File(excelStream, Constants.ContentTypeExcel, "3-color Heatmap.xlsx");
    }

    private IReportTable<ReportCell> BuildReport()
    {
        ThreeColorHeatmapProperty heatmapProperty = new(0, Color.Red, 50, Color.Yellow, 100, Color.Lime);

        ReportSchemaBuilder<Entity> reportBuilder = new();
        reportBuilder.AddColumn("Name", e => e.Name);
        reportBuilder.AddColumn("Score", e => e.Score)
            .AddProperties(heatmapProperty);

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
        ReportConverter<ExcelReportCell> excelConverter = new(new[]
        {
            new ThreeColorHeatmapPropertyExcelHandler(),
        });

        return excelConverter.Convert(reportTable);
    }

    private string WriteHtmlReportToString(IReportTable<HtmlReportCell> htmlReportTable)
    {
        return new HtmlStringWriter(new HtmlStringCellWriter()).Write(htmlReportTable);
    }

    private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
    {
        return new EpplusWriter().WriteToStream(reportTable);
    }

    private IEnumerable<Entity> GetData()
    {
        return new Faker<Entity>()
            .RuleFor(e => e.Name, f => f.Name.FullName())
            .RuleFor(e => e.Score, f => f.Random.Decimal(0, 100))
            .Generate(RecordsCount);
    }

    private class Entity
    {
        public string Name { get; set; }

        public decimal Score { get; set; }
    }

    private class ThreeColorHeatmapProperty : ReportCellProperty
    {
        private readonly decimal minimumValue;
        private readonly Color minimumColor;
        private readonly decimal middleValue;
        private readonly Color middleColor;
        private readonly decimal maximumValue;
        private readonly Color maximumColor;

        public ThreeColorHeatmapProperty(decimal minimumValue, Color minimumColor, decimal middleValue, Color middleColor, decimal maximumValue, Color maximumColor)
        {
            this.minimumValue = minimumValue;
            this.minimumColor = minimumColor;
            this.middleValue = middleValue;
            this.middleColor = middleColor;
            this.maximumValue = maximumValue;
            this.maximumColor = maximumColor;
        }

        public Color GetColorForValue(decimal value)
        {
            if (value < this.middleValue)
            {
                return this.GetColorForValue(value, this.minimumValue, this.minimumColor, this.middleValue, this.middleColor);
            }
            else
            {
                return this.GetColorForValue(value, this.middleValue, this.middleColor, this.maximumValue, this.maximumColor);
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

    private class ThreeColorHeatmapPropertyExcelHandler : PropertyHandler<ThreeColorHeatmapProperty, ExcelReportCell>
    {
        protected override void HandleProperty(ThreeColorHeatmapProperty property, ExcelReportCell cell)
        {
            decimal value = cell.GetValue<decimal>();
            cell.BackgroundColor = property.GetColorForValue(value);
        }
    }
}
