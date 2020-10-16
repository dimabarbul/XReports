using System;
using Reports.Factories;
using Reports.Interfaces;

namespace Reports.Models.Columns
{
    public class EntityPropertyReportColumn<TSourceEntity, TValue> : ReportColumn<TSourceEntity>
    {
        public override string Title { get; }
        public override Func<TSourceEntity, IReportCell> CellSelector {
            get
            {
                return entity => this.DecorateCell(ReportCellFactory.Create<TValue>(this.valueSelector(entity)));
            }
        }

        private readonly Func<TSourceEntity, TValue> valueSelector;

        public EntityPropertyReportColumn(string title, Func<TSourceEntity, TValue> valueSelector)
        {
            this.Title = title;
            this.valueSelector = valueSelector;
        }
    }
}
