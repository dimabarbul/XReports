using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportCellsProvider<in TSourceEntity>
    {
        ReportCell GetCell(TSourceEntity entity);
    }
}
