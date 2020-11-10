using System.Collections.Generic;
using System.Linq;
using Reports.Models;

namespace Reports.Tests
{
    public partial class VerticalReportTest
    {
        private ReportCell[][] GetCellsAsArray(IEnumerable<IEnumerable<ReportCell>> cells)
        {
            return cells.Select(row => row.ToArray()).ToArray();
        }
    }
}
