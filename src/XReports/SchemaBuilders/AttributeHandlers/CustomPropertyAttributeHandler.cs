using System;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

namespace XReports.SchemaBuilders.AttributeHandlers
{
    public class CustomPropertyAttributeHandler : AttributeHandler<CustomPropertyAttribute>
    {
        protected override void HandleAttribute<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> builder,
            IReportColumnBuilder<TSourceEntity> columnBuilder,
            CustomPropertyAttribute attribute)
        {
            ReportCellProperty property = (ReportCellProperty)Activator.CreateInstance(attribute.PropertyType);

            if (attribute.IsHeader)
            {
                columnBuilder.AddHeaderProperties(property);
            }
            else
            {
                columnBuilder.AddProperties(property);
            }
        }
    }
}
