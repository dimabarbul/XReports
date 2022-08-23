using System.Data;
using XReports.BenchmarksCore.ReportStructure.Models.Properties;

namespace XReports.BenchmarksCore.ReportStructure.Models;

public class ReportCellsSourceFromDataReader : ReportCellsSource
{
    public ReportCellsSourceFromDataReader(string title, Func<IDataReader, dynamic> valueSelector, params ReportCellsSourceProperty[] properties)
        : base(title, properties)
    {
        this.ValueSelector = valueSelector;
    }

    public Func<IDataReader, dynamic> ValueSelector { get; }
}
