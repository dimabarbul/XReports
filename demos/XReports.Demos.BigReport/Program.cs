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
        Person[] data = DataProvider.GetData(1_000);
        DataTable dataTable = DataProvider.CreateDataTable(data);

        ReportService reportService = new(data, dataTable);

        File.Delete("/tmp/report.xlsx");

        Stopwatch sw = Stopwatch.StartNew();
        await reportService.VerticalFromDataReaderExcelToFileAsync("/tmp/report.xlsx");
        sw.Stop();
        Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
    }
}
