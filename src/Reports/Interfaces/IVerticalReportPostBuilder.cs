using Reports.SchemaBuilders;

namespace Reports.Interfaces
{
    public interface IVerticalReportPostBuilder<TSourceEntity>
    {
        void Build(VerticalReportSchemaBuilder<TSourceEntity> builder);
    }
}
