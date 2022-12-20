using XReports.Models;

namespace XReports.Tests.Assertions
{
    internal static class ReportCellAssertionsExtensions
    {
        public static ReportCellAssertions Should(this ReportCell reportCell, string identifier = null)
        {
            return identifier == null ?
                new ReportCellAssertions(reportCell) :
                new ReportCellAssertions(reportCell, identifier);
        }
    }
}
