using System.Collections.Generic;
using XReports.Table;

namespace XReports.Tests.Common.Assertions
{
    public static class ReportCellPropertiesAssertionsExtensions
    {
        public static ReportCellPropertiesAssertions Should(this IEnumerable<IReportCellProperty> properties)
        {
            return new ReportCellPropertiesAssertions(properties);
        }
    }
}
