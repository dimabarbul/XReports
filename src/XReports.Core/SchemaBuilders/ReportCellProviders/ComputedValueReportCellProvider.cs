using System;
using XReports.Table;

namespace XReports.SchemaBuilders.ReportCellProviders
{
    public class ComputedValueReportCellProvider<TSourceEntity, TValue> : ReportCellProvider<TSourceEntity, TValue>
    {
        private readonly Func<TSourceEntity, TValue> valueSelector;

        public ComputedValueReportCellProvider(Func<TSourceEntity, TValue> valueSelector)
        {
            this.valueSelector = valueSelector;
        }

        public override ReportCell GetCell(TSourceEntity entity)
        {
            return this.CreateCell(this.valueSelector(entity));
        }
    }
}
