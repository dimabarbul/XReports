using System.IO;
using System.Threading.Tasks;

namespace XReports.Html.Writers
{
    public interface IHtmlStreamCellWriter
    {
        Task WriteHeaderCellAsync(StreamWriter streamWriter, HtmlReportCell cell);

        Task WriteBodyCellAsync(StreamWriter streamWriter, HtmlReportCell cell);
    }
}
