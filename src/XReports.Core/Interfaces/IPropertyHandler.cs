using XReports.Models;

namespace XReports.Interfaces
{
    public interface IPropertyHandler<in TResultReportCell>
    {
        int Priority { get; }

        bool Handle(ReportCellProperty property, TResultReportCell cell);
    }
}
