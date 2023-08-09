using XReports.Table;

namespace XReports.Benchmarks.ReportStructure.Models;

public abstract class ReportCellsSource<TSourceEntity>
{
    protected ReportCellsSource(string title, params IReportCellProperty[] properties)
    {
        this.Title = title;
        this.Properties = properties;
    }

    public string Title { get; }

    public IReportCellProperty[] Properties { get; }

    public abstract Type ValueType { get; }
    public abstract Func<TSourceEntity, TValue> ConvertValueSelector<TValue>();
}
