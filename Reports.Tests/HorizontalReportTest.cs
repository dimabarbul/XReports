using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;

namespace Reports.Tests
{
    public partial class HorizontalReportTest
    {
        private IReportCell[][] GetCellsAsArray(IEnumerable<IEnumerable<IReportCell>> cells)
        {
            return cells.Select(row => row.ToArray()).ToArray();
        }
    }
}
