using XReports.Benchmarks.Core.ReportStructure.Models.Properties;

namespace XReports.Benchmarks.Core.ReportStructure.Models;

public class TypedReportCellsSource<TSourceEntity, TValue> : ReportCellsSource<TSourceEntity>
{
    public TypedReportCellsSource(string title, Func<TSourceEntity, TValue> valueSelector, params ReportCellsSourceProperty[] properties)
        : base(title, properties)
    {
        this.ValueSelector = valueSelector;
    }

    public Func<TSourceEntity, TValue> ValueSelector { get; }

    public override Type ValueType => typeof(TValue);

    public override Func<TSourceEntity, TRequestedValue> ConvertValueSelector<TRequestedValue>()
    {
        if (typeof(TValue) != typeof(TRequestedValue))
        {
            throw new ArgumentException($"Wrong requested value type: requested={typeof(TRequestedValue)}, actual={typeof(TValue)}");
        }

        return this.ValueSelector as Func<TSourceEntity, TRequestedValue>
            ?? throw new ArgumentException($"Value selector of type {this.ValueSelector.GetType()} cannot be converted to type {typeof(Func<TSourceEntity, TRequestedValue>)}");
    }
}
