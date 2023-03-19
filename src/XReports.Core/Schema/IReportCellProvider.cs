using XReports.Table;

namespace XReports.Schema
{
    public interface IReportCellProvider<in TSourceEntity>
    {
        ReportCell GetCell(TSourceEntity entity);
    }
}
