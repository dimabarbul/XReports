using System.Collections.Generic;
using Reports.Interfaces;

namespace Reports.Models
{
    public class ReportTable
    {
        public List<List<IReportCell>> Cells { get; set; } = new List<List<IReportCell>>();
    }
}
