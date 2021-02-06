using System.Diagnostics.CodeAnalysis;

namespace XReports.Models
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Classes have the same name")]
    public class ReportCell : BaseReportCell
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Classes have the same name")]
    public class ReportCell<TValue> : ReportCell
    {
        public ReportCell(TValue value)
        {
            this.Value = value;
            this.ValueType = typeof(TValue);
        }
    }
}
