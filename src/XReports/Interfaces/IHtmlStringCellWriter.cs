using System.Text;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IHtmlStringCellWriter
    {
        void WriteHeaderCell(StringBuilder stringBuilder, HtmlReportCell cell);

        void WriteBodyCell(StringBuilder stringBuilder, HtmlReportCell cell);
    }
}
