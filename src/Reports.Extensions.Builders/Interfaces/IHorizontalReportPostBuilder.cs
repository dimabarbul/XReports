using Reports.SchemaBuilders;

namespace Reports.Extensions.Builders.Interfaces
{
    public interface IHorizontalReportPostBuilder
    {
        void Build<TSourceEntity>(HorizontalReportSchemaBuilder<TSourceEntity> builder);
    }
}
