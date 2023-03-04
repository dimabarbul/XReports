using System.Collections.Generic;

namespace XReports.Core.Tests.Assertions
{
    internal static class NewReportCellsAssertionsExtensions
    {
        public static NewReportCellsAssertions Should(this IEnumerable<IEnumerable<ReportConverterTest.NewReportCell>> reportCells)
        {
            return new NewReportCellsAssertions(reportCells);
        }
    }
}
