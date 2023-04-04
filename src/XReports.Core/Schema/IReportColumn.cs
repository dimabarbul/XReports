using XReports.Table;

namespace XReports.Schema
{
    /// <summary>
    /// Interface for report column with data source item of type <typeparamref name="TSourceItem"/>.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    public interface IReportColumn<in TSourceItem>
    {
        /// <summary>
        /// Creates cell based on data source item.
        /// </summary>
        /// <param name="item">Data source item.</param>
        /// <returns>Report cell.</returns>
        ReportCell CreateCell(TSourceItem item);

        /// <summary>
        /// Creates report column header cell.
        /// </summary>
        /// <returns>Header cell.</returns>
        ReportCell CreateHeaderCell();
    }
}
