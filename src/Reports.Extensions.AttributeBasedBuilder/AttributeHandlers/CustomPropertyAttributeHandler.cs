using System;
using Reports.Core.Models;
using Reports.Core.SchemaBuilders;
using Reports.Extensions.AttributeBasedBuilder.Attributes;

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
