using System.Diagnostics;
using Bogus;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.PropertyHandlers;
using XReports.SchemaBuilders;
using XReports.Writers;

namespace XReports.Demos.BigReport;

internal static class Program
{
    public static async Task Main()
    {
        ReportBuilder builder = new(1_000_000);

        Stream stream = new MemoryStream();

        Stopwatch sw = Stopwatch.StartNew();
        await builder.ToStreamAsync(stream);
        sw.Stop();
        Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");

        stream.Seek(0, SeekOrigin.Begin);
        FileStream fileStream = File.OpenWrite("/tmp/report.html");
        await stream.CopyToAsync(fileStream);
        fileStream.Close();
    }
}


public class ReportBuilder
{
    private readonly int recordsCount;

    public ReportBuilder(int recordsCount)
    {
        this.recordsCount = recordsCount;
    }

    public Task EnumAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);

        foreach (ReportTableProperty _ in htmlReportTable.Properties)
        {
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.HeaderRows)
        {
            foreach (HtmlReportCell cell in row)
            {
            }
        }

        foreach (IEnumerable<HtmlReportCell> row in htmlReportTable.Rows)
        {
            foreach (HtmlReportCell cell in row)
            {
            }
        }

        return Task.CompletedTask;
    }

    public string ToString()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        return this.WriteReportToString(htmlReportTable);
    }

    public Task ToStreamAsync(Stream stream)
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        return this.WriteReportToStreamAsync(htmlReportTable, stream);
    }

    public async Task ToFileAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        Stream excelStream = this.WriteExcelReportToStream(excelReportTable);

        FileStream fileStream = File.OpenWrite("/tmp/File.xlsx");

        await excelStream.CopyToAsync(fileStream);
        fileStream.Close();
    }

    private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
    {
        return new EpplusWriter().WriteToStream(reportTable);
    }

    private IReportTable<ReportCell> BuildReport()
    {
        CustomFormatProperty customFormatProperty = new();
        VerticalReportSchemaBuilder<Entity> reportBuilder = new();
        reportBuilder.AddColumn("Name", e => e.Name);
        reportBuilder.AddColumn("Full Name", e => e.Name);
        reportBuilder.AddColumn("Score", e => e.Score)
            .AddProperties(customFormatProperty);
        reportBuilder.AddColumn("Average Score", e => e.Score)
            .AddProperties(customFormatProperty);
        reportBuilder.AddColumn("Minimum Score", e => e.Score)
            .AddProperties(customFormatProperty);
        reportBuilder.AddColumn("Maximum Score", e => e.Score)
            .AddProperties(customFormatProperty);

        IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(this.GetData());
        return reportTable;
    }

    private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
    {
        ReportConverter<HtmlReportCell> htmlConverter = new(new[]
        {
            new CustomFormatPropertyHtmlHandler(),
        });

        return htmlConverter.Convert(reportTable);
    }

    private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
    {
        ReportConverter<ExcelReportCell> excelConverter = new(new[]
        {
            new CustomFormatPropertyExcelHandler(),
        });

        return excelConverter.Convert(reportTable);
    }

    private string WriteReportToString(IReportTable<HtmlReportCell> htmlReportTable)
    {
        return new Writers.StringWriter(new StringCellWriter()).WriteToString(htmlReportTable);
    }

    private Task WriteReportToStreamAsync(IReportTable<HtmlReportCell> htmlReportTable, Stream stream)
    {
        return new Writers.StreamWriter(new StreamCellWriter()).WriteAsync(htmlReportTable, stream);
    }

    private IEnumerable<Entity> GetData()
    {
        int luckyGuyIndex = new Random().Next(3, recordsCount - 1);

        return new Faker<Entity>()
            .RuleFor(e => e.Name, f => f.Name.FullName())
            .RuleFor(e => e.Score, f => f.IndexFaker % luckyGuyIndex == 0 ? 100m : f.Random.Decimal(80, 100))
            .Generate(recordsCount);
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

    private class CustomFormatProperty : ReportCellProperty
    {
    }

    private class CustomFormatPropertyHtmlHandler : PropertyHandler<CustomFormatProperty, HtmlReportCell>
    {
        protected override void HandleProperty(CustomFormatProperty property, HtmlReportCell cell)
        {
            decimal value = cell.GetValue<decimal>();
            string format = value == 100m ? "F0" : "F2";

            cell.SetValue(value.ToString(format));
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
