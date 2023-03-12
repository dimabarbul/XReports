using System;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

namespace XReports.SchemaBuilders.AttributeHandlers
{
    public class CustomPropertyAttributeHandler : AttributeHandler<CustomPropertyAttribute>
    {
        protected override void HandleAttribute<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> builder,
            IReportColumnBuilder<TSourceEntity> cellsProviderBuilder,
            CustomPropertyAttribute attribute)
        {
            ReportCellProperty property = (ReportCellProperty)Activator.CreateInstance(attribute.PropertyType);

            if (attribute.IsHeader)
            {
                cellsProviderBuilder.AddHeaderProperties(property);
            }
            else
            {
                cellsProviderBuilder.AddProperties(property);
            }
        }
    }
}
