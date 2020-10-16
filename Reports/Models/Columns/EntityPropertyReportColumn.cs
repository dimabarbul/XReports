using System;
using System.Linq.Expressions;
using Reports.Interfaces;
using Reports.Models.Cells;

namespace Reports.Models.Columns
{
    public class EntityPropertyReportColumn<TSourceEntity, TValueType> : IReportColumn<TSourceEntity>
    {
        public string Title { get; }
        public Func<TSourceEntity, IReportCell> CellSelector {
            get
            {
                return entity => new StaticTextReportCell(this.ValueSelector(entity).ToString());
            }
        }

        private Func<TSourceEntity, TValueType> ValueSelector { get; set; }

        public EntityPropertyReportColumn(string title, Func<TSourceEntity, TValueType> valueSelector)
        {
            this.Title = title;
            this.ValueSelector = valueSelector;
        }
    }
}
