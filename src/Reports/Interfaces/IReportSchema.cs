using System.Collections.Generic;
using Reports.Models;

namespace Reports.Interfaces
{
    public interface IReportSchema<TSourceEntity>
    {
        IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source);
    }
}
