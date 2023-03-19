using XReports.ReportCellProperties;
using XReports.SchemaBuilders.Attributes;

namespace XReports.SchemaBuilders.AttributeHandlers
{
    public class SameColumnFormatAttributeHandler : AttributeHandler<SameColumnFormatAttribute>
    {
        protected override void HandleAttribute<TSourceEntity>(
            IReportSchemaBuilder<TSourceEntity> builder,
            IReportColumnBuilder<TSourceEntity> columnBuilder,
            SameColumnFormatAttribute attribute)
        {
            columnBuilder.AddProperties(new SameColumnFormatProperty());
        }
    }
}
