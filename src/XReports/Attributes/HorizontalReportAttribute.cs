using XReports.Enums;

namespace XReports.Attributes
{
    public sealed class HorizontalReportAttribute : ReportAttribute
    {
        public HorizontalReportAttribute()
            : base(ReportType.Horizontal)
        {
        }
    }
}
