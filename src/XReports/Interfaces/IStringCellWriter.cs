using System.Text;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IStringCellWriter
    {
        void WriteHeaderCell(StringBuilder stringBuilder, HtmlReportCell cell);

        void WriteBodyCell(StringBuilder stringBuilder, HtmlReportCell cell);
    }
}
