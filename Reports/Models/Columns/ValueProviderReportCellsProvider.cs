using System;
using Reports.Interfaces;

namespace Reports.Models.Columns
{
    public class ValueProviderReportCellsProvider<TSourceEntity, TValue> : ReportCellsProvider<TSourceEntity, TValue>
    {
        public override Func<TSourceEntity, IReportCell> CellSelector
        {
            get
            {
                return entity => this.CreateCell(this.provider.GetValue());
            }
        }

        private readonly IValueProvider<TValue> provider;

        public ValueProviderReportCellsProvider(string title, IValueProvider<TValue> provider)
            : base(title)
        {
            this.provider = provider;
        }
    }
}
