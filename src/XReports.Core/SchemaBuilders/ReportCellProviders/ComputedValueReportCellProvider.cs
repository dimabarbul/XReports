using System;
using XReports.Table;

namespace XReports.SchemaBuilders.ReportCellProviders
{
    /// <summary>
    /// Provider of cells which value is computed based on data source item.
    /// </summary>
    /// <typeparam name="TSourceItem">Type of data source item.</typeparam>
    /// <typeparam name="TValue">Type of cell value.</typeparam>
    public class ComputedValueReportCellProvider<TSourceItem, TValue> : ReportCellProvider<TSourceItem, TValue>
    {
        private readonly Func<TSourceItem, TValue> valueSelector;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputedValueReportCellProvider{TSourceItem, TValue}"/> class.
        /// </summary>
        /// <param name="valueSelector">Function that returns cell value based on data source item.</param>
        public ComputedValueReportCellProvider(Func<TSourceItem, TValue> valueSelector)
        {
            this.valueSelector = valueSelector;
        }

        /// <inheritdoc />
        public override ReportCell GetCell(TSourceItem item)
        {
            return this.GetCell(this.valueSelector(item));
        }
    }
}
