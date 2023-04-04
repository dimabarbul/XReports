using XReports.Table;

namespace XReports.Schema
{
    /// <summary>
    /// Interface for class providing report cells based on data source item.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    public interface IReportCellProvider<in TSourceItem>
    {
        /// <summary>
        /// Returns report cell based on data source item.
        /// </summary>
        /// <param name="item">Data source item.</param>
        /// <returns>Report cell.</returns>
        ReportCell GetCell(TSourceItem item);
    }
}
