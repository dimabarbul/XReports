using XReports.Models;
using XReports.Table;

namespace XReports.Interfaces
{
    public interface IHtmlStringWriter
    {
        string WriteToString(IReportTable<HtmlReportCell> reportTable);
    }
}
