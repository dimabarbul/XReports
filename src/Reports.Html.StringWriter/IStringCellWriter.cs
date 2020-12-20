using Reports.Core.Models;

namespace Reports.Html.StringWriter
{
    public interface IStringCellWriter
    {
        string WriteHeaderCell(HtmlReportCell cell);
        string WriteBodyCell(HtmlReportCell cell);
    }
}
