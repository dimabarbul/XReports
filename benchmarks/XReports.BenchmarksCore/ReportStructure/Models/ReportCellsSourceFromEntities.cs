using XReports.BenchmarksCore.Models;
using XReports.BenchmarksCore.ReportStructure.Models.Properties;

namespace XReports.BenchmarksCore.ReportStructure.Models;

public class ReportCellsSourceFromEntities<TValue> : BaseReportCellsSourceFromEntities
{
    public ReportCellsSourceFromEntities(string title, Func<Person, TValue> valueSelector, params ReportCellsSourceProperty[] properties)
        : base(title, properties)
    {
        this.ValueSelector = valueSelector;
    }

    public Func<Person, TValue> ValueSelector { get; }

    public override Func<Person, TRequestedValue> GetValueSelector<TRequestedValue>()
    {
        if (typeof(TValue) != typeof(TRequestedValue))
        {
            throw new ArgumentException($"Wrong requested value type: requested={typeof(TRequestedValue)}, actual={typeof(TValue)}");
        }

        return this.ValueSelector as Func<Person, TRequestedValue>;
    }

    public override Type GetValueType()
    {
        return typeof(TValue);
    }
}