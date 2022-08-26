using System.Data;
using System.Diagnostics;
using XReports.Benchmarks.Core;
using XReports.Benchmarks.Core.Models;
using XReports.Benchmarks.NewVersion;

Person[] data = DataProvider.GetData(10_000);
DataTable dataTable = DataProvider.CreateDataTable(data);

ReportService reportService = new(data, dataTable);

string fileName = Path.GetTempFileName();

Stopwatch sw = Stopwatch.StartNew();
await reportService.VerticalFromEntitiesHtmlEnumAsync();
sw.Stop();
Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");

if (File.Exists(fileName))
{
    File.Delete(fileName);
}
