using XReports.Table;

namespace XReports.Converter
{
    public interface IPropertyHandler<in TResultReportCell>
        where TResultReportCell : ReportCell
    {
        int Priority { get; }

        bool Handle(ReportCellProperty property, TResultReportCell cell);
    }
}
