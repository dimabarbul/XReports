using XReports.Table;

namespace XReports.Converter
{
    public interface IReportConverter<out TResultReportCell>
        where TResultReportCell : ReportCell
    {
        IReportTable<TResultReportCell> Convert(IReportTable<ReportCell> table);
    }
}
