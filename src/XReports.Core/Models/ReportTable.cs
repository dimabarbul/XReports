using System.Collections.Generic;
using XReports.Interfaces;

namespace XReports.Models
{
    public class ReportTable<TReportCell> : IReportTable<TReportCell>
    {
        public IEnumerable<IEnumerable<TReportCell>> HeaderRows { get; set; }

        public IEnumerable<IEnumerable<TReportCell>> Rows { get; set; }
    }
}
