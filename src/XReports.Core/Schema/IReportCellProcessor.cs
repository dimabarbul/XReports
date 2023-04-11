using XReports.Table;

namespace XReports.Schema
{
    /// <summary>
    /// Interface for report cell processor.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    public interface IReportCellProcessor<in TSourceItem>
    {
        /// <summary>
        /// Processes report cell based on data source item. Unlike property handlers which have access only to report cell value and the its properties, processor has access to data source item which was used to build the report cell.
        /// </summary>
        /// <param name="cell">Report cell to process.</param>
        /// <param name="item">Data source item.</param>
        void Process(ReportCell cell, TSourceItem item);
    }
}
