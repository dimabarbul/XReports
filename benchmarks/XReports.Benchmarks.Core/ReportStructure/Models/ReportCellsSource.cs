using XReports.Benchmarks.Core.ReportStructure.Models.Properties;

namespace XReports.Benchmarks.Core.ReportStructure.Models;

public abstract class ReportCellsSource<TSourceEntity>
{
    protected ReportCellsSource(string title, params ReportCellsSourceProperty[] properties)
    {
        this.Title = title;
        this.Properties = properties;
    }

    public string Title { get; }

    public IEnumerable<ReportCellsSourceProperty> Properties { get; }

    public abstract Type ValueType { get; }
    public abstract Func<TSourceEntity, TValue> ConvertValueSelector<TValue>();
}
