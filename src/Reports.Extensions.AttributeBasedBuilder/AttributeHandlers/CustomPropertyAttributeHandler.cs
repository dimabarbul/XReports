using System;
using Reports.Extensions.AttributeBasedBuilder.Attributes;
using Reports.Models;
using Reports.SchemaBuilders;

namespace Reports.Extensions.AttributeBasedBuilder.AttributeHandlers
{
    public class CustomPropertyAttributeHandler : AttributeHandler<CustomPropertyAttribute>
    {
        protected override void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, CustomPropertyAttribute attribute)
        {
            ReportCellProperty property = (ReportCellProperty) Activator.CreateInstance(attribute.PropertyType);

            if (attribute.IsHeader)
            {
                builder.AddHeaderProperties(property);
            }
            else
            {
                builder.AddProperties(property);
            }
        }
    }
}
