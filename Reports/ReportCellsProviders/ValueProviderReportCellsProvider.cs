using System;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.ReportCellsProviders
{
    internal class ValueProviderReportCellsProvider<TSourceEntity, TValue> : ReportCellsProvider<TSourceEntity, TValue>
    {
        public override Func<TSourceEntity, ReportCell> CellSelector
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
