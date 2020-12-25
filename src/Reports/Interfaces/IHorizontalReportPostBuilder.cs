using Reports.SchemaBuilders;

namespace Reports.Interfaces
{
    public interface IHorizontalReportPostBuilder<TSourceEntity>
    {
        void Build(HorizontalReportSchemaBuilder<TSourceEntity> builder);
    }
}
