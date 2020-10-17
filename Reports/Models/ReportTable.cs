using System.Collections.Generic;
using Reports.Interfaces;

namespace Reports.Models
{
    public class ReportTable
    {
        public ICollection<IEnumerable<IReportCell>> HeaderCells { get; set; } = new List<IEnumerable<IReportCell>>();
        public ICollection<IEnumerable<IReportCell>> Cells { get; set; } = new List<IEnumerable<IReportCell>>();
    }
}
