using Reports.SchemaBuilders;

namespace Reports.Extensions.AttributeBasedBuilder.Interfaces
{
    public interface IHorizontalReportPostBuilder
    {
        void Build<TSourceEntity>(HorizontalReportSchemaBuilder<TSourceEntity> builder);
    }
}
