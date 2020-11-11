using System;
using Reports.Models;

namespace Reports.ReportCellsProviders
{
    public class EntityPropertyReportCellsProvider<TSourceEntity, TValue> : ReportCellsProvider<TSourceEntity, TValue>
    {
        public override Func<TSourceEntity, ReportCell> CellSelector {
            get
            {
                return entity => this.CreateCell(this.valueSelector(entity));
            }
        }

        private readonly Func<TSourceEntity, TValue> valueSelector;

        public EntityPropertyReportCellsProvider(string title, Func<TSourceEntity, TValue> valueSelector)
            : base(title)
        {
            this.valueSelector = valueSelector;
        }
    }
}
