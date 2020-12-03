using Reports.SchemaBuilders;

namespace Reports.Extensions.AttributeBasedBuilder.Interfaces
{
    public interface IVerticalReportPostBuilder
    {
        void Build<TSourceEntity>(VerticalReportSchemaBuilder<TSourceEntity> builder);
    }
}
