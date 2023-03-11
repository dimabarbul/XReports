using XReports.Attributes;
using XReports.Properties;
using XReports.SchemaBuilder;

namespace XReports.AttributeHandlers
{
    public class SameColumnFormatAttributeHandler : AttributeHandler<SameColumnFormatAttribute>
    {
        protected override void HandleAttribute<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> builder,
            IReportColumnBuilder<TSourceEntity> cellsProviderBuilder,
            SameColumnFormatAttribute attribute)
        {
            cellsProviderBuilder.AddProperties(new SameColumnFormatProperty());
        }
    }
}
