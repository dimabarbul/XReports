using System.Collections.Generic;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportSchema<in TSourceEntity>
    {
        IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source);
    }
}
