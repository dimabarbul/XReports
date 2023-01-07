using System;
using XReports.Models;

namespace XReports.ReportCellsProviders
{
    public class ComputedValueReportCellsProvider<TSourceEntity, TValue> : ReportCellsProvider<TSourceEntity, TValue>
    {
        private readonly Func<TSourceEntity, TValue> valueSelector;

        public ComputedValueReportCellsProvider(Func<TSourceEntity, TValue> valueSelector)
        {
            this.valueSelector = valueSelector;
        }

        public override ReportCell GetCell(TSourceEntity entity)
        {
            return this.CreateCell(this.valueSelector(entity));
        }
    }
}
