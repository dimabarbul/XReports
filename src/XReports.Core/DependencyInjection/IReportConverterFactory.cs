using XReports.Converter;
using XReports.Table;

namespace XReports.DependencyInjection
{
    public interface IReportConverterFactory<out TReportCell>
        where TReportCell : ReportCell, new()
    {
        IReportConverter<TReportCell> Get(string name);
    }
}
