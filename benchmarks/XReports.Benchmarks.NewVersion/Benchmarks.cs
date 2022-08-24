using XReports.Benchmarks.Core;

namespace XReports.Benchmarks.NewVersion;

public class Benchmarks : BenchmarksBase
{
    protected override ReportService CreateReportService()
    {
        if (this.data is null || this.dataTable is null)
        {
            throw new InvalidOperationException("Data or data reader is not initialized.");
        }

        return new ReportService(this.data, this.dataTable);
    }
}
