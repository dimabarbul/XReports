using XReports.Attributes;
using XReports.Properties;
using XReports.SchemaBuilders;

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
