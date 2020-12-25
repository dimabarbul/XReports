using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportConverter<out TResultReportCell>
        where TResultReportCell : BaseReportCell, new()
    {
        IReportTable<TResultReportCell> Convert(IReportTable<ReportCell> table);
    }
}
