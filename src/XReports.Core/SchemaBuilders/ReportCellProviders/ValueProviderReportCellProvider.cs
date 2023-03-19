using XReports.Table;

namespace XReports.SchemaBuilders.ReportCellProviders
{
    public class ValueProviderReportCellProvider<TSourceEntity, TValue> : ReportCellProvider<TSourceEntity, TValue>
    {
        private readonly IValueProvider<TValue> provider;

        public ValueProviderReportCellProvider(IValueProvider<TValue> provider)
        {
            this.provider = provider;
        }

        public override ReportCell GetCell(TSourceEntity entity)
        {
            return this.CreateCell(this.provider.GetValue());
        }
    }
}
