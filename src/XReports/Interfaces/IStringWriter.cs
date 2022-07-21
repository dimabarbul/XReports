using XReports.Models;

namespace XReports.Interfaces
{
    public interface IStringWriter
    {
        string WriteToString(IReportTable<HtmlReportCell> reportTable);
    }
}
