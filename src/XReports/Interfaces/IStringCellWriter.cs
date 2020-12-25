using XReports.Models;

namespace XReports.Interfaces
{
    public interface IStringCellWriter
    {
        string WriteHeaderCell(HtmlReportCell cell);
        string WriteBodyCell(HtmlReportCell cell);
    }
}
