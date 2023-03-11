using System;
using XReports.Attributes;
using XReports.SchemaBuilder;
using XReports.Table;

namespace XReports.AttributeHandlers
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
