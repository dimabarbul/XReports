using System.Diagnostics.CodeAnalysis;
using XReports.Models;
using XReports.SchemaBuilder;

namespace XReports.Interfaces
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
