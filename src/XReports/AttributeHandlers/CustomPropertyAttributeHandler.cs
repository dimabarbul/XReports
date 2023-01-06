using System;
using XReports.Attributes;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.AttributeHandlers
{
    public class CustomPropertyAttributeHandler : AttributeHandler<CustomPropertyAttribute>
    {
        protected override void HandleAttribute<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> builder,
            IReportSchemaCellsProviderBuilder<TSourceEntity> cellsProviderBuilder,
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
