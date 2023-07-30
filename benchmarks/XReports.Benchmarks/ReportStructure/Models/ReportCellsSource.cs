using XReports.Table;

namespace XReports.Benchmarks.ReportStructure.Models;

public abstract class ReportCellsSource<TSourceEntity>
{
    protected ReportCellsSource(string title, params ReportCellProperty[] properties)
    {
        this.Title = title;
        this.Properties = properties;
    }

    public string Title { get; }

    public ReportCellProperty[] Properties { get; }

    public abstract Type ValueType { get; }
    public abstract Func<TSourceEntity, TValue> ConvertValueSelector<TValue>();
}
