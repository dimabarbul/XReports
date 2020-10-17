using System;
using Reports.Interfaces;

namespace Reports.Models.Columns
{
    public class ComputedValueProviderReportColumn<TSourceEntity, TValue> : ReportColumn<TSourceEntity, TValue>
    {
        public override Func<TSourceEntity, IReportCell> CellSelector
        {
            get
            {
                return entity => this.CreateCell(this.provider.GetValue(entity));
            }
        }

        private readonly IComputedValueProvider<TSourceEntity, TValue> provider;

        public ComputedValueProviderReportColumn(string title, IComputedValueProvider<TSourceEntity, TValue> provider)
            : base(title)
        {
            this.provider = provider;
        }
    }
}
