using System.Data;
using System.Diagnostics;
using XReports.BenchmarksCore;
using XReports.BenchmarksCore.Models;
using XReports.NewVersion;

namespace XReports.Demos.BigReport;

internal static class Program
{
    public static async Task Main()
    {
        Person[] data = DataProvider.GetData(10_000);
        DataTable dataTable = DataProvider.CreateDataTable(data);

        ReportService reportService = new(data, dataTable);

        const string fileName = "/tmp/report";

        File.Delete(fileName);

        Stopwatch sw = Stopwatch.StartNew();
        await reportService.VerticalFromEntitiesHtmlEnumAsync();
        sw.Stop();
        Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
    }
}
