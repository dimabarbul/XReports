using XReports.Table;

namespace XReports.Schema
{
    /// <summary>
    /// Interface for processors of header report cells.
    /// </summary>
    public interface IHeaderReportCellProcessor
    {
        /// <summary>
        /// Processes header report cell.
        /// </summary>
        /// <param name="cell">Report cell to process.</param>
        void Process(ReportCell cell);
    }
}
