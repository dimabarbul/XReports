using System.Data;
using BenchmarkDotNet.Attributes;
using XReports.Benchmarks.Core.Interfaces;
using XReports.Benchmarks.Core.Models;

namespace XReports.Benchmarks.Core;

[MemoryDiagnoser]
public abstract class BenchmarksBase
{
    protected Person[] Data { get; private set; }
    protected DataTable Table { get; private set; }

    [ParamsSource(nameof(GetRowCounts))]
    public int RowCount { get; set; }

    public IEnumerable<int> GetRowCounts()
    {
        string sizes = Environment.GetEnvironmentVariable("RowCounts") ?? "10000";

        return sizes
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse);
    }

    [GlobalSetup]
    public void GlobalSetup()
    {
        this.Data = DataProvider.GetData(this.RowCount).ToArray();
        this.Table = DataProvider.CreateDataTable(this.Data);
    }

    [Benchmark(Description = "Enumerate vertical HTML report from entities without saving anywhere")]
    public async Task VerticalFromEntitiesHtmlEnumAsync()
    {
        await this.CreateReportService().VerticalFromEntitiesHtmlEnumAsync();
    }

    [Benchmark(Description = "Save vertical HTML report from entities to string using StringBuilder")]
    public async Task VerticalFromEntitiesHtmlToStringAsync()
    {
        await this.CreateReportService().VerticalFromEntitiesHtmlToStringAsync();
    }

    [Benchmark(Description = "Save vertical HTML report from entities to file")]
    public async Task VerticalFromEntitiesHtmlToFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.CreateReportService().VerticalFromEntitiesHtmlToFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    [Benchmark(Description = "Enumerate vertical XLSX report from entities without saving anywhere")]
    public async Task VerticalFromEntitiesExcelEnumAsync()
    {
        await this.CreateReportService().VerticalFromEntitiesExcelEnumAsync();
    }

    [Benchmark(Description = "Save vertical XLSX report from entities to file")]
    public async Task VerticalFromEntitiesExcelToFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.CreateReportService().VerticalFromEntitiesExcelToFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    [Benchmark(Description = "Save vertical XLSX report from entities to stream")]
    public async Task VerticalFromEntitiesExcelToStreamAsync()
    {
        await this.CreateReportService().VerticalFromEntitiesExcelToStreamAsync();
    }

    [Benchmark(Description = "Enumerate vertical HTML report from data reader without saving anywhere")]
    public async Task VerticalFromDataReaderHtmlEnumAsync()
    {
        await this.CreateReportService().VerticalFromDataReaderHtmlEnumAsync();
    }

    [Benchmark(Description = "Save vertical HTML report from data reader to string using StringBuilder")]
    public async Task VerticalFromDataReaderHtmlToStringAsync()
    {
        await this.CreateReportService().VerticalFromDataReaderHtmlToStringAsync();
    }

    [Benchmark(Description = "Save vertical HTML report from data reader to file")]
    public async Task VerticalFromDataReaderHtmlToFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.CreateReportService().VerticalFromDataReaderHtmlToFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    [Benchmark(Description = "Enumerate vertical XLSX report from data reader without saving anywhere")]
    public async Task VerticalFromDataReaderExcelEnumAsync()
    {
        await this.CreateReportService().VerticalFromDataReaderExcelEnumAsync();
    }

    [Benchmark(Description = "Save vertical XLSX report from data reader to file")]
    public async Task VerticalFromDataReaderExcelToFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.CreateReportService().VerticalFromDataReaderExcelToFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    [Benchmark(Description = "Save vertical XLSX report from data reader to stream")]
    public async Task VerticalFromDataReaderExcelToStreamAsync()
    {
        await this.CreateReportService().VerticalFromDataReaderExcelToStreamAsync();
    }

    [Benchmark(Description = "Enumerate horizontal HTML report without saving anywhere")]
    public async Task HorizontalHtmlEnumAsync()
    {
        await this.CreateReportService().HorizontalHtmlEnumAsync();
    }

    [Benchmark(Description = "Save horizontal HTML report to string using StringBuilder")]
    public async Task HorizontalHtmlToStringAsync()
    {
        await this.CreateReportService().HorizontalHtmlToStringAsync();
    }

    [Benchmark(Description = "Save horizontal HTML report to file")]
    public async Task HorizontalHtmlToFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.CreateReportService().HorizontalHtmlToFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    [Benchmark(Description = "Enumerate horizontal XLSX report without saving anywhere")]
    public async Task HorizontalExcelEnumAsync()
    {
        await this.CreateReportService().HorizontalExcelEnumAsync();
    }

    protected abstract IReportService CreateReportService();
}
