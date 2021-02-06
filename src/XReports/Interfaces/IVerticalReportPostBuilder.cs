using System.Diagnostics.CodeAnalysis;
using XReports.SchemaBuilders;

namespace XReports.Interfaces
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Interfaces have the same name")]
    public interface IVerticalReportPostBuilder<TSourceEntity>
    {
        void Build(VerticalReportSchemaBuilder<TSourceEntity> builder);
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Interfaces have the same name")]
    public interface IVerticalReportPostBuilder<TSourceEntity, in TBuildParameter>
    {
        void Build(VerticalReportSchemaBuilder<TSourceEntity> builder, TBuildParameter parameter);
    }
}
