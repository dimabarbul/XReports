using System.Collections.Generic;
using Reports.Core.Models;

namespace Reports.Core.Interfaces
{
    public interface IReportSchema<in TSourceEntity>
    {
        IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source);
    }
}
