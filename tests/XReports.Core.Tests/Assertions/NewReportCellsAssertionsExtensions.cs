using System.Collections.Generic;
using XReports.Core.Tests.Converter;

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
