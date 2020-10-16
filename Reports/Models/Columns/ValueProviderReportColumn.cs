using System;
using Reports.Interfaces;
using Reports.Models.Cells;

namespace Reports.Models.Columns
{
    public class ValueProviderReportColumn<TSourceEntity> : IReportColumn<TSourceEntity>
    {
        public string Title { get; }

        public Func<TSourceEntity, IReportCell> CellSelector
        {
            get
            {
                return entity => new TextReportCell(this.provider.GetValue());
            }
        }

        private readonly IValueProvider provider;

        public ValueProviderReportColumn(string title, IValueProvider provider)
        {
            this.Title = title;
            this.provider = provider;
        }
    }
}
