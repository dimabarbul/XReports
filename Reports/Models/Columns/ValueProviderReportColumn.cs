using System;
using Reports.Factories;
using Reports.Interfaces;

namespace Reports.Models.Columns
{
    public class ValueProviderReportColumn<TSourceEntity> : ReportColumn<TSourceEntity>
    {
        public override string Title { get; }

        public override Func<TSourceEntity, IReportCell> CellSelector
        {
            get
            {
                return entity => this.DecorateCell(ReportCellFactory.Create(this.provider.GetValue()));
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
