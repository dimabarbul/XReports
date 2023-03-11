using XReports.Table;

namespace XReports.Tests.Common.Assertions
{
    public static class ReportCellAssertionsExtensions
    {
        public static ReportCellAssertions Should(this ReportCell reportCell, string identifier = null)
        {
            return identifier == null ?
                new ReportCellAssertions(reportCell) :
                new ReportCellAssertions(reportCell, identifier);
        }
    }
}
