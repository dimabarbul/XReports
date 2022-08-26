using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportCellsProvider<in TSourceEntity>
    {
        string Title { get; }

        ReportCell GetCell(TSourceEntity entity);
    }
}
