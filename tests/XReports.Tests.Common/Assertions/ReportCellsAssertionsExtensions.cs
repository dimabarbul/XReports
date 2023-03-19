using System.Collections.Generic;
using XReports.Table;

namespace XReports.Tests.Common.Assertions
{
    public static class ReportCellsAssertionsExtensions
    {
        public static ReportCellsAssertions Should(this IEnumerable<IEnumerable<ReportCell>> reportCells)
        {
            return new ReportCellsAssertions(reportCells);
        }
    }
}
