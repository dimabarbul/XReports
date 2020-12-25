using System;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Attributes;

namespace XReports.AttributeHandlers
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
