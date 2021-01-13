using XReports.Models;

namespace XReports.Interfaces
{
    public interface IReportConverterFactory<out TReportCell>
        where TReportCell : BaseReportCell, new()

    {
        IReportConverter<TReportCell> Get(string name);
    }
}
