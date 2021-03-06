using System;
using XReports.Models;

namespace XReports.ReportCellsProviders
{
    public class ComputedValueReportCellsProvider<TSourceEntity, TValue> : ReportCellsProvider<TSourceEntity, TValue>
    {
        private readonly Func<TSourceEntity, TValue> valueSelector;

        public ComputedValueReportCellsProvider(string title, Func<TSourceEntity, TValue> valueSelector)
            : base(title)
        {
            this.valueSelector = valueSelector;
        }

        public override Func<TSourceEntity, ReportCell> CellSelector
        {
            get
            {
                return entity => this.CreateCell(this.valueSelector(entity), entity);
            }
        }
    }
}
