using XReports.Benchmarks.Core;

namespace XReports.Benchmarks.NewVersion;

public class ReportServiceBenchmarks : BenchmarksBase
{
    protected override ReportService CreateReportService()
    {
        if (this.Data is null || this.Table is null)
        {
            throw new InvalidOperationException("Data or data reader is not initialized.");
        }

        return new ReportService(this.Data, this.Table);
    }
}