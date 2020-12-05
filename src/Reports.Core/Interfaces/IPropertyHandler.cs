using Reports.Core.Models;

namespace Reports.Core.Interfaces
{
    public interface IPropertyHandler<in TResultReportCell>
    {
        int Priority { get; }
        void Handle(ReportCellProperty property, TResultReportCell cell);
    }
}
