using XReports.BenchmarksCore.Models;
using XReports.BenchmarksCore.ReportStructure.Models.Properties;

namespace XReports.BenchmarksCore.ReportStructure.Models;

public abstract class BaseReportCellsSourceFromEntities : ReportCellsSource
{
    protected BaseReportCellsSourceFromEntities(string title, params ReportCellsSourceProperty[] properties)
        : base(title, properties)
    {
    }

    public abstract Func<Person, TValue> GetValueSelector<TValue>();
    public abstract Type GetValueType();
}
