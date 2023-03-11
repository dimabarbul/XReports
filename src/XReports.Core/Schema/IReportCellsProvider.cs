using XReports.Table;

namespace XReports.Schema
{
    public interface IReportCellsProvider<in TSourceEntity>
    {
        ReportCell GetCell(TSourceEntity entity);
    }
}
