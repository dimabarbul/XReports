using System.Collections.Generic;
using XReports.Table;

namespace XReports.Schema
{
    public interface IReportSchema<in TSourceEntity>
    {
        IReportTable<ReportCell> BuildReportTable(IEnumerable<TSourceEntity> source);
    }
}
