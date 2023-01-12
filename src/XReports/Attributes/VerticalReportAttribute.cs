using XReports.Enums;

namespace XReports.Attributes
{
    public sealed class VerticalReportAttribute : ReportAttribute
    {
        public VerticalReportAttribute()
            : base(ReportType.Vertical)
        {
        }
    }
}
