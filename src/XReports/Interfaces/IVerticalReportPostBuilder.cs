using XReports.SchemaBuilders;

namespace XReports.Interfaces
{
    public interface IVerticalReportPostBuilder<TSourceEntity>
    {
        void Build(VerticalReportSchemaBuilder<TSourceEntity> builder);
    }

    public interface IVerticalReportPostBuilder<TSourceEntity, in TBuildParameter>
    {
        void Build(VerticalReportSchemaBuilder<TSourceEntity> builder, TBuildParameter parameter);
    }
}
