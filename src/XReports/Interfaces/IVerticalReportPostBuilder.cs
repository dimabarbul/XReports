using System.Diagnostics.CodeAnalysis;

namespace XReports.Interfaces
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Interfaces have the same name")]
    public interface IVerticalReportPostBuilder<TSourceEntity>
    {
        void Build(IVerticalReportSchemaBuilder<TSourceEntity> builder);
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Interfaces have the same name")]
    public interface IVerticalReportPostBuilder<TSourceEntity, in TBuildParameter>
    {
        void Build(IVerticalReportSchemaBuilder<TSourceEntity> builder, TBuildParameter parameter);
    }
}
