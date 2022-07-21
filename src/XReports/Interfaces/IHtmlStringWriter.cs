using XReports.Models;

namespace XReports.Interfaces
{
    public interface IHtmlStringWriter
    {
        string WriteToString(IReportTable<HtmlReportCell> reportTable);
    }
}
