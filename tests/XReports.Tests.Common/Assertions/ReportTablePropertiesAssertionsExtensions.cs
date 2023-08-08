using System.Collections.Generic;
using XReports.Table;

namespace XReports.Tests.Common.Assertions
{
    public static class ReportTablePropertiesAssertionsExtensions
    {
        public static ReportTablePropertiesAssertions Should(this IEnumerable<IReportTableProperty> properties)
        {
            return new ReportTablePropertiesAssertions(properties);
        }
    }
}
