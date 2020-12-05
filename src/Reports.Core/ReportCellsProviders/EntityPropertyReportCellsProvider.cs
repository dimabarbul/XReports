using System;
using Reports.Core.Models;

namespace Reports.Core.ReportCellsProviders
{
    public class EntityPropertyReportCellsProvider<TSourceEntity, TValue> : ReportCellsProvider<TSourceEntity, TValue>
    {
        public override Func<TSourceEntity, ReportCell> CellSelector {
            get
            {
                return entity => this.CreateCell(this.valueSelector(entity), entity);
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
