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

        public override ReportCell GetCell(TSourceEntity entity)
        {
            return this.CreateCell(this.provider.GetValue(), entity);
        }
    }
}
