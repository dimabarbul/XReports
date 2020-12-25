using XReports.SchemaBuilders;
using XReports.Attributes;
using XReports.Properties;

namespace XReports.AttributeHandlers
{
    public class SameColumnFormatAttributeHandler : AttributeHandler<SameColumnFormatAttribute>
    {
        protected override void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, SameColumnFormatAttribute attribute)
        {
            builder.AddProperties(new SameColumnFormatProperty());
        }
    }
}
