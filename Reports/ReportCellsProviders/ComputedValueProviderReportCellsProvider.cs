using System;
using Reports.Interfaces;

namespace Reports.ReportCellsProviders
{
    internal class ComputedValueProviderReportCellsProvider<TSourceEntity, TValue> : ReportCellsProvider<TSourceEntity, TValue>
    {
        public override Func<TSourceEntity, IReportCell> CellSelector
        {
            get
            {
                return entity => this.CreateCell(this.provider.GetValue(entity));
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
