using System.Collections.Generic;
using XReports.Models;

namespace XReports.Tests.Assertions
{
    internal static class ReportCellsAssertionsExtensions
    {
        public static ReportCellsAssertions Should(this IEnumerable<IEnumerable<ReportCell>> reportCells)
        {
            return new ReportCellsAssertions(reportCells);
        }
    }
}
