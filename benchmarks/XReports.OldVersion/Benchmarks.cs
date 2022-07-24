using BenchmarkDotNet.Attributes;
using XReports.BenchmarksCore;

namespace XReports.OldVersion;

[MemoryDiagnoser]
public class Benchmarks : BenchmarksBase
{
    [Benchmark(Description = "Enumerate report without saving anywhere")]
    public async Task EnumAsync()
    {
        await this.CreateReportService().EnumAsync();
    }

    [Benchmark(Description = "Save to string using StringBuilder")]
    public async Task ToStringAsync()
    {
        await this.CreateReportService().ToStringAsync();
    }

    [Benchmark(Description = "Save to XLSX file")]
    public async Task ToExcelFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.CreateReportService().ToExcelFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    [Benchmark(Description = "Save to XLSX stream")]
    public async Task ToExcelStreamAsync()
    {
        await this.CreateReportService().ToExcelStreamAsync();
    }

    [Benchmark(Description = "Save to HTML file")]
    public async Task ToHtmlFileAsync()
    {
        string fileName = Path.GetTempFileName();

        await this.CreateReportService().ToHtmlFileAsync(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }

    private ReportService CreateReportService()
    {
        if (this.data is null)
        {
            throw new InvalidOperationException("Data is not initialized.");
        }

        return new ReportService(this.data);
    }
}
