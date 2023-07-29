using System.Data;
using BenchmarkDotNet.Attributes;
using XReports.Benchmarks.Models;

namespace XReports.Benchmarks;

[GcServer]
[MemoryDiagnoser]
public class ReportServiceBenchmarks : IDisposable
{
    private IReadOnlyList<Person> data;
    private DataTable table;
    private ReportService reportService;
    private bool disposed;

    [ParamsSource(nameof(GetRowCounts))]
    public int RowCount { get; set; }

    public static IEnumerable<int> GetRowCounts()
    {
        string sizes = Environment.GetEnvironmentVariable("RowCounts") ?? "10000";

        return sizes
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse);
    }

    [GlobalSetup]
    public void GlobalSetup()
    {
        this.data = DataProvider.GetData(this.RowCount);
        this.table = DataProvider.CreateDataTable(this.data);

        this.reportService = new ReportService(this.data, this.table);
    }

    [Benchmark(Description = "Enumerate vertical HTML report from entities without saving anywhere")]
    public async Task VerticalFromEntitiesHtmlEnumAsync()
    {
        await this.reportService.VerticalFromEntitiesHtmlEnumAsync();
    }

    [Benchmark(Description = "Save vertical HTML report from entities to string using StringBuilder")]
    public async Task VerticalFromEntitiesHtmlToStringAsync()
    {
        await this.reportService.VerticalFromEntitiesHtmlToStringAsync();
    }

    [Benchmark(Description = "Save vertical HTML report from entities to file")]
    public async Task VerticalFromEntitiesHtmlToFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.reportService.VerticalFromEntitiesHtmlToFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    [Benchmark(Description = "Enumerate vertical XLSX report from entities without saving anywhere")]
    public async Task VerticalFromEntitiesExcelEnumAsync()
    {
        await this.reportService.VerticalFromEntitiesExcelEnumAsync();
    }

    [Benchmark(Description = "Save vertical XLSX report from entities to file")]
    public async Task VerticalFromEntitiesExcelToFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.reportService.VerticalFromEntitiesExcelToFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    [Benchmark(Description = "Save vertical XLSX report from entities to stream")]
    public async Task VerticalFromEntitiesExcelToStreamAsync()
    {
        await this.reportService.VerticalFromEntitiesExcelToStreamAsync();
    }

    [Benchmark(Description = "Enumerate vertical HTML report from data reader without saving anywhere")]
    public async Task VerticalFromDataReaderHtmlEnumAsync()
    {
        await this.reportService.VerticalFromDataReaderHtmlEnumAsync();
    }

    [Benchmark(Description = "Save vertical HTML report from data reader to string using StringBuilder")]
    public async Task VerticalFromDataReaderHtmlToStringAsync()
    {
        await this.reportService.VerticalFromDataReaderHtmlToStringAsync();
    }

    [Benchmark(Description = "Save vertical HTML report from data reader to file")]
    public async Task VerticalFromDataReaderHtmlToFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.reportService.VerticalFromDataReaderHtmlToFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    [Benchmark(Description = "Enumerate vertical XLSX report from data reader without saving anywhere")]
    public async Task VerticalFromDataReaderExcelEnumAsync()
    {
        await this.reportService.VerticalFromDataReaderExcelEnumAsync();
    }

    [Benchmark(Description = "Save vertical XLSX report from data reader to file")]
    public async Task VerticalFromDataReaderExcelToFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.reportService.VerticalFromDataReaderExcelToFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    [Benchmark(Description = "Save vertical XLSX report from data reader to stream")]
    public async Task VerticalFromDataReaderExcelToStreamAsync()
    {
        await this.reportService.VerticalFromDataReaderExcelToStreamAsync();
    }

    [Benchmark(Description = "Enumerate horizontal HTML report without saving anywhere")]
    public async Task HorizontalHtmlEnumAsync()
    {
        await this.reportService.HorizontalHtmlEnumAsync();
    }

    [Benchmark(Description = "Save horizontal HTML report to string using StringBuilder")]
    public async Task HorizontalHtmlToStringAsync()
    {
        await this.reportService.HorizontalHtmlToStringAsync();
    }

    [Benchmark(Description = "Save horizontal HTML report to file")]
    public async Task HorizontalHtmlToFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.reportService.HorizontalHtmlToFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    [Benchmark(Description = "Enumerate horizontal XLSX report without saving anywhere")]
    public async Task HorizontalExcelEnumAsync()
    {
        await this.reportService.HorizontalExcelEnumAsync();
    }

    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        GC.SuppressFinalize(this);

        this.reportService.Dispose();
        this.disposed = true;
    }
}
