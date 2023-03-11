using XReports.Table;

namespace XReports.Schema
{
    public interface IReportColumn<in TSourceEntity>
    {
        ReportCell CreateCell(TSourceEntity entity);
        ReportCell CreateHeaderCell();
    }
}
