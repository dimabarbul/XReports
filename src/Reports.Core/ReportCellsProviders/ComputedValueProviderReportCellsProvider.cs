using System;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.ReportCellsProviders
{
    public class ComputedValueProviderReportCellsProvider<TSourceEntity, TValue> : ReportCellsProvider<TSourceEntity, TValue>
    {
        public override Func<TSourceEntity, ReportCell> CellSelector
        {
            get
            {
                return entity => this.CreateCell(this.provider.GetValue(entity), entity);
            }
        }

        private readonly IComputedValueProvider<TSourceEntity, TValue> provider;

        public ComputedValueProviderReportCellsProvider(string title, IComputedValueProvider<TSourceEntity, TValue> provider)
            : base(title)
        {
            this.provider = provider;
        }
    }
}
