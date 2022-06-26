using System.Diagnostics.CodeAnalysis;

namespace XReports.Models
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Classes have the same name")]
    public class ReportCell : BaseReportCell
    {
        public static readonly ReportCell EmptyCell = new ReportCell<string>(string.Empty);
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
