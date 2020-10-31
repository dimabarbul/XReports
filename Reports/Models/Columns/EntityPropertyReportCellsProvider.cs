using System;
using Reports.Interfaces;

namespace Reports.Models.Columns
{
    public class EntityPropertyReportCellsProvider<TSourceEntity, TValue> : ReportCellsProvider<TSourceEntity, TValue>
    {
        public override Func<TSourceEntity, IReportCell> CellSelector {
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
