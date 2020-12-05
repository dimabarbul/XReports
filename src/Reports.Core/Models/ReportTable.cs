using System.Collections.Generic;
using Reports.Core.Interfaces;

namespace Reports.Core.Models
{
    public class ReportTable<TReportCell> : IReportTable<TReportCell>
    {
        public IEnumerable<IEnumerable<TReportCell>> HeaderRows { get; set; }
        public IEnumerable<IEnumerable<TReportCell>> Rows { get; set; }
    }
}
