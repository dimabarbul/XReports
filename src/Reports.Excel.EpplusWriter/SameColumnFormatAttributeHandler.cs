using Reports.Core.SchemaBuilders;
using Reports.Extensions.AttributeBasedBuilder.AttributeHandlers;

namespace Reports.Excel.EpplusWriter
{
    public class SameColumnFormatAttributeHandler : AttributeHandler<SameColumnFormatAttribute>
    {
        protected override void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, SameColumnFormatAttribute attribute)
        {
            builder.AddProperties(new SameColumnFormatProperty());
        }
    }
}
