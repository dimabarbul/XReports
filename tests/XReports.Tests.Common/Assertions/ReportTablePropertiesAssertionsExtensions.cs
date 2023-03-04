using System.Collections.Generic;
using XReports.Models;

namespace XReports.Tests.Common.Assertions
{
    public static class ReportTablePropertiesAssertionsExtensions
    {
        public static ReportTablePropertiesAssertions Should(this IEnumerable<ReportTableProperty> properties)
        {
            return new ReportTablePropertiesAssertions(properties);
        }
    }
}
