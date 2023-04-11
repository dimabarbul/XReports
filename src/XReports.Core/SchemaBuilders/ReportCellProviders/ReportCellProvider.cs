using XReports.Schema;
using XReports.Table;

namespace XReports.SchemaBuilders.ReportCellProviders
{
    /// <summary>
    /// Base cell provider that caches cells, i.e., instead of creating new cells it reuses the same cell.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    /// <typeparam name="TValue">Type of cell value.</typeparam>
    public abstract class ReportCellProvider<TSourceItem, TValue> : IReportCellProvider<TSourceItem>
    {
        private readonly ReportCell reportCell = new ReportCell();

        /// <inheritdoc />
        public abstract ReportCell GetCell(TSourceItem item);

        /// <summary>
        /// Returns cell with the value.
        /// </summary>
        /// <remarks>The method does not create new cell, instead it returns same cell, but cleared and with new value. So if the cell needs to be saved for later processing, use <see cref="ReportCell.Clone"/>.</remarks>
        /// <param name="value">Cell value.</param>
        /// <returns>Cell with the value.</returns>
        protected ReportCell GetCell(TValue value)
        {
            this.reportCell.Clear();
            this.reportCell.SetValue(value);

            return this.reportCell;
        }
    }
}
