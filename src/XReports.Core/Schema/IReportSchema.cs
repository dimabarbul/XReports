using System.Collections.Generic;
using XReports.Table;

namespace XReports.Schema
{
    /// <summary>
    /// Interface for schema of report that uses data source item of type <typeparamref name="TSourceItem"/>.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    public interface IReportSchema<in TSourceItem>
    {
        /// <summary>
        /// Build report table using data from <paramref name="source"/>.
        /// </summary>
        /// <param name="source">Data source.</param>
        /// <returns>Report table with cells of type <see cref="ReportCell"/>.</returns>
        IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceItem> source);
    }
}
