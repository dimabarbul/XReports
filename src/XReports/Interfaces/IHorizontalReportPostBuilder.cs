using XReports.SchemaBuilders;

namespace XReports.Interfaces
{
    public interface IHorizontalReportPostBuilder<TSourceEntity>
    {
        void Build(HorizontalReportSchemaBuilder<TSourceEntity> builder);
    }

    public interface IHorizontalReportPostBuilder<TSourceEntity, in TBuildParameter>
    {
        void Build(HorizontalReportSchemaBuilder<TSourceEntity> builder, TBuildParameter parameter);
    }
}
