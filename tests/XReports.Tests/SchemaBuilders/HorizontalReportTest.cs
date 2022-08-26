using System.Collections.Generic;
using System.Linq;
using XReports.Models;

namespace XReports.Tests.SchemaBuilders
{
    public partial class HorizontalReportTest
    {
        private ReportCell[][] GetCellsAsArray(IEnumerable<IEnumerable<ReportCell>> cells)
        {
            return cells.Select(row => row.Select(c => c?.Clone() as ReportCell).ToArray()).ToArray();
        }
    }
}
