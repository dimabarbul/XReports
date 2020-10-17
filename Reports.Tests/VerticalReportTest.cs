using System.Collections.Generic;
using System.Linq;
using Reports.Interfaces;

namespace Reports.Tests
{
    public partial class VerticalReportTest
    {
        private IReportCell[][] GetCellsAsArray(ICollection<IEnumerable<IReportCell>> cells)
        {
            return cells.Select(row => row.ToArray()).ToArray();
        }
    }
}
