using System.Diagnostics;
using Bogus;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.Properties;
using XReports.PropertyHandlers;
using XReports.SchemaBuilders;
using XReports.Writers;

namespace XReports.Demos.BigReport;

internal static class Program
{
    public static async Task Main()
    {
        ReportBuilder builder = new(1_000_000);

        Stopwatch sw = Stopwatch.StartNew();
        builder.ToExcelFileAsFileStream();
        sw.Stop();
        Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
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

    public Task ToStreamAsync(StreamWriter streamWriter)
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
        return this.WriteReportToStreamAsync(htmlReportTable, streamWriter);
    }

    public async Task ToExcelFileAsStreamAsync()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        Stream excelStream = this.WriteExcelReportToStream(excelReportTable);

        FileStream fileStream = File.Create("/tmp/report.xlsx");

        await excelStream.CopyToAsync(fileStream);
        fileStream.Close();
    }

    public void ToExcelFileAsFileStream()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        FileStream fileStream = File.Create("/tmp/report.xlsx");
        this.WriteExcelReportToStream(excelReportTable, fileStream);

        fileStream.Close();
    }

    public void ToExcelFile()
    {
        IReportTable<ReportCell> reportTable = this.BuildReport();
        IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

        this.WriteExcelReportToFile(excelReportTable);
    }

    private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
    {
        return new EpplusWriter().WriteToStream(reportTable);
    }

    private void WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable, Stream stream)
    {
        new EpplusWriter().WriteToStream(reportTable, stream);
    }

    private void WriteExcelReportToFile(IReportTable<ExcelReportCell> reportTable)
    {
        const string fileName = "/tmp/report.xlsx";
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        new EpplusWriter().WriteToFile(reportTable, fileName);
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

        reportBuilder.AddGlobalProperties(new SameColumnFormatProperty());

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
        return new HtmlStringWriter(new HtmlStringCellWriter()).WriteToString(htmlReportTable);
    }

    private Task WriteReportToStreamAsync(IReportTable<HtmlReportCell> htmlReportTable, Stream stream)
    {
        return new HtmlStreamWriter(new HtmlStreamCellWriter()).WriteAsync(htmlReportTable, stream);
    }

    private Task WriteReportToStreamAsync(IReportTable<HtmlReportCell> htmlReportTable, StreamWriter streamWriter)
    {
        return new HtmlStreamWriter(new HtmlStreamCellWriter()).WriteAsync(htmlReportTable, streamWriter);
    }

    private IEnumerable<Entity> GetData()
    {
        int luckyGuyIndex = new Random().Next(3, this.recordsCount - 1);

        return new Faker<Entity>()
            .RuleFor(e => e.Name, f => f.Name.FullName())
            .RuleFor(e => e.Score, f => f.IndexFaker % luckyGuyIndex == 0 ? 100m : f.Random.Decimal(80, 100))
            .Generate(this.recordsCount);
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
