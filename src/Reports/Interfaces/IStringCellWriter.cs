using Reports.Models;

namespace Reports.Interfaces
{
    public interface IStringCellWriter
    {
        string WriteHeaderCell(HtmlReportCell cell);
        string WriteBodyCell(HtmlReportCell cell);
    }
}
