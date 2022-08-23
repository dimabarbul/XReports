using XReports.BenchmarksCore.ReportStructure.Models.Properties;

namespace XReports.BenchmarksCore.ReportStructure.Models;

public abstract class ReportCellsSource
{
    protected ReportCellsSource(string title, params ReportCellsSourceProperty[] properties)
    {
        this.Title = title;
        this.Properties = properties;
    }

    public string Title { get; }

    public IEnumerable<ReportCellsSourceProperty> Properties { get; }
}
