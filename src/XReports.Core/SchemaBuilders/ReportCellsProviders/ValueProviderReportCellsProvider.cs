using XReports.Table;

namespace XReports.SchemaBuilders.ReportCellsProviders
{
    public class ValueProviderReportCellsProvider<TSourceEntity, TValue> : ReportCellsProvider<TSourceEntity, TValue>
    {
        private readonly IValueProvider<TValue> provider;

        public ValueProviderReportCellsProvider(IValueProvider<TValue> provider)
        {
            this.provider = provider;
        }

        public override ReportCell GetCell(TSourceEntity entity)
        {
            return this.CreateCell(this.provider.GetValue());
        }
    }
}
