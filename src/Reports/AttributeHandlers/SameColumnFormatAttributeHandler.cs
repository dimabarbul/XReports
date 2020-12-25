using Reports.Attributes;
using Reports.Properties;
using Reports.SchemaBuilders;

namespace Reports.AttributeHandlers
{
    public class SameColumnFormatAttributeHandler : AttributeHandler<SameColumnFormatAttribute>
    {
        protected override void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, SameColumnFormatAttribute attribute)
        {
            builder.AddProperties(new SameColumnFormatProperty());
        }
    }
}
