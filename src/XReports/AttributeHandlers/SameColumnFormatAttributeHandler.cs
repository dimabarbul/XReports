using XReports.Attributes;
using XReports.Interfaces;
using XReports.Properties;

namespace XReports.AttributeHandlers
{
    public class SameColumnFormatAttributeHandler : AttributeHandler<SameColumnFormatAttribute>
    {
        protected override void HandleAttribute<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> builder,
            IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
            SameColumnFormatAttribute attribute)
        {
            cellsProviderBuilder.AddProperties(new SameColumnFormatProperty());
        }
    }
}
