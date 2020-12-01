using Reports.SchemaBuilders;

namespace Reports.Extensions.Builders.Interfaces
{
    public interface IVerticalReportPostBuilder
    {
        void Build<TSourceEntity>(VerticalReportSchemaBuilder<TSourceEntity> builder);
    }
}
