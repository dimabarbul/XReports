using BenchmarkDotNet.Attributes;
using XReports.BenchmarksCore.Models;

namespace XReports.BenchmarksCore;

public class BenchmarksBase
{
    protected Person[]? data;

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
        this.data = new DataProvider().GetData(this.RowCount).ToArray();
    }
}
