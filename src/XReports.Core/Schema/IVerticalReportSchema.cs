using System.Data;
using XReports.Table;

namespace XReports.Schema
{
    public interface IVerticalReportSchema<in TSourceEntity> : IReportSchema<TSourceEntity>
    {
        IReportTable<ReportCell> BuildReportTable(IDataReader dataReader);
    }
}
