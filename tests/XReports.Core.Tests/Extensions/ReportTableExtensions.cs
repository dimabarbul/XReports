using System.Collections.Generic;
using XReports.Table;

namespace XReports.Core.Tests.Extensions
{
    internal static class ReportTableExtensions
    {
        public static void Enumerate<T>(this IReportTable<T> table)
            where T : ReportCell
        {
            foreach (IEnumerable<T> row in table.HeaderRows)
            {
                foreach (T _ in row)
                {
                }
            }

            foreach (IEnumerable<T> row in table.Rows)
            {
                foreach (T _ in row)
                {
                }
            }
        }
    }
}
