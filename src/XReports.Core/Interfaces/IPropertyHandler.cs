using XReports.Models;

namespace XReports.Interfaces
{
    public interface IPropertyHandler<in TResultReportCell>
    {
        int Priority { get; }
        void Handle(ReportCellProperty property, TResultReportCell cell);
    }
}
