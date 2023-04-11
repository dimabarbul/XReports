using XReports.ReportCellProperties;
using XReports.SchemaBuilders.Attributes;

namespace XReports.SchemaBuilders.AttributeHandlers
{
    /// <summary>
    /// Handler of <see cref="SameColumnFormatAttribute"/>. Assigns <see cref="SameColumnFormatProperty"/> to column with the attribute.
    /// </summary>
    public class SameColumnFormatAttributeHandler : AttributeHandler<SameColumnFormatAttribute>
    {
        /// <inheritdoc />
        protected override void HandleAttribute<TSourceItem>(
            IReportSchemaBuilder<TSourceItem> schemaBuilder,
            IReportColumnBuilder<TSourceItem> columnBuilder,
            SameColumnFormatAttribute attribute)
        {
            columnBuilder.AddProperties(new SameColumnFormatProperty());
        }
    }
}
