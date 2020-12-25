using XReports.SchemaBuilders;

namespace XReports.Interfaces
{
    public interface IHorizontalReportPostBuilder<TSourceEntity>
    {
        void Build(HorizontalReportSchemaBuilder<TSourceEntity> builder);
    }
}
