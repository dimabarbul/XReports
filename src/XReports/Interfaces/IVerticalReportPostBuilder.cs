using XReports.SchemaBuilders;

namespace XReports.Interfaces
{
    public interface IVerticalReportPostBuilder<TSourceEntity>
    {
        void Build(VerticalReportSchemaBuilder<TSourceEntity> builder);
    }
}