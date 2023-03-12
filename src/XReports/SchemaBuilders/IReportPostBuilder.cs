using System.Diagnostics.CodeAnalysis;

namespace XReports.SchemaBuilders
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Interfaces have the same name")]
    public interface IReportPostBuilder<TSourceEntity>
    {
        void Build(IReportSchemaBuilder<TSourceEntity> builder, BuildOptions options);
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Interfaces have the same name")]
    public interface IReportPostBuilder<TSourceEntity, in TBuildParameter>
    {
        void Build(IReportSchemaBuilder<TSourceEntity> builder, TBuildParameter parameter, BuildOptions options);
    }
}
