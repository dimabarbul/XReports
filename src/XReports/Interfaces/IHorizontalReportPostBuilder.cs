using System.Diagnostics.CodeAnalysis;
using XReports.SchemaBuilders;

namespace XReports.Interfaces
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Interfaces have the same name")]
    public interface IHorizontalReportPostBuilder<TSourceEntity>
    {
        void Build(HorizontalReportSchemaBuilder<TSourceEntity> builder);
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Interfaces have the same name")]
    public interface IHorizontalReportPostBuilder<TSourceEntity, in TBuildParameter>
    {
        void Build(HorizontalReportSchemaBuilder<TSourceEntity> builder, TBuildParameter parameter);
    }
}
