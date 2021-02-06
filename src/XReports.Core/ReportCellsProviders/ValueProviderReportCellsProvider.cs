using System;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.ReportCellsProviders
{
    public class ValueProviderReportCellsProvider<TSourceEntity, TValue> : ReportCellsProvider<TSourceEntity, TValue>
    {
        private readonly IValueProvider<TValue> provider;

        public ValueProviderReportCellsProvider(string title, IValueProvider<TValue> provider)
            : base(title)
        {
            this.provider = provider;
        }

        public override Func<TSourceEntity, ReportCell> CellSelector
        {
            get
            {
                return entity => this.CreateCell(this.provider.GetValue(), entity);
            }
        }
    }
}
