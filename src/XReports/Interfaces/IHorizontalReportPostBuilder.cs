using System.Diagnostics.CodeAnalysis;

namespace XReports.Interfaces
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Interfaces have the same name")]
    public interface IHorizontalReportPostBuilder<TSourceEntity>
    {
        void Build(IHorizontalReportSchemaBuilder<TSourceEntity> builder);
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Interfaces have the same name")]
    public interface IHorizontalReportPostBuilder<TSourceEntity, in TBuildParameter>
    {
        void Build(IHorizontalReportSchemaBuilder<TSourceEntity> builder, TBuildParameter parameter);
    }
}
