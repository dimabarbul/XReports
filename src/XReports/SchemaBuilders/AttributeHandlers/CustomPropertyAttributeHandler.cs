using System;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

namespace XReports.SchemaBuilders.AttributeHandlers
{
    /// <summary>
    /// Handler of <see cref="CustomPropertyAttribute"/>. Assigns property of type specified by <see cref="CustomPropertyAttribute.PropertyType"/>.
    /// </summary>
    public class CustomPropertyAttributeHandler : AttributeHandler<CustomPropertyAttribute>
    {
        /// <inheritdoc />
        protected override void HandleAttribute<TSourceItem>(
            IReportSchemaBuilder<TSourceItem> schemaBuilder,
            IReportColumnBuilder<TSourceItem> columnBuilder,
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
