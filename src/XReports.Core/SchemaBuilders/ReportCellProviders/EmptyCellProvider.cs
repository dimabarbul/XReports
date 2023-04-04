using XReports.Table;

namespace XReports.SchemaBuilders.ReportCellProviders
{
    /// <summary>
    /// Provider of empty cells.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    public class EmptyCellProvider<TSourceItem> : ReportCellProvider<TSourceItem, string>
    {
        /// <inheritdoc/>
        public override ReportCell GetCell(TSourceItem item)
        {
            return this.GetCell(string.Empty);
        }
    }
}
