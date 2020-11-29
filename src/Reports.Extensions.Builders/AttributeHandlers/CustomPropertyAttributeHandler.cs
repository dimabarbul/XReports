using System;
using Reports.Extensions.Builders.Attributes;
using Reports.Models;
using Reports.SchemaBuilders;

namespace Reports.Extensions.Builders.AttributeHandlers
{
    public class CustomPropertyAttributeHandler : EntityAttributeHandler<CustomPropertyAttribute>
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
