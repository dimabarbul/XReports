using System.Collections.Generic;
using Reports.Interfaces;

namespace Reports.Models
{
    internal class ReportTable : IReportTable
    {
        public IEnumerable<IEnumerable<IReportCell>> HeaderRows { get; set; }
        public IEnumerable<IEnumerable<IReportCell>> Rows { get; set; }
    }
}
