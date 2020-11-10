using System.Collections.Generic;
using Reports.Interfaces;

namespace Reports.Models
{
    public class ReportTable<TReportCell> : IReportTable<TReportCell>
    {
        public IEnumerable<IEnumerable<TReportCell>> HeaderRows { get; set; }
        public IEnumerable<IEnumerable<TReportCell>> Rows { get; set; }
    }
}
