using System.Collections.Generic;
using System.Linq;
using XReports.Models;

namespace XReports.Tests.Common.Assertions
{
    public static class ReportCellsExtensions
    {
        public static TReportCell[][] Clone<TReportCell>(this IEnumerable<IEnumerable<TReportCell>> cells)
            where TReportCell : BaseReportCell
        {
            return cells.Select(row => row.Select(c => (TReportCell)c?.Clone()).ToArray()).ToArray();
        }
    }
}
