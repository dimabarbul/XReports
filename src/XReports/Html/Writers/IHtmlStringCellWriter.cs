using System.Text;

namespace XReports.Html.Writers
{
    public interface IHtmlStringCellWriter
    {
        void WriteHeaderCell(StringBuilder stringBuilder, HtmlReportCell cell);

        void WriteBodyCell(StringBuilder stringBuilder, HtmlReportCell cell);
    }
}
