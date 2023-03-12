using System.IO;
using System.Threading.Tasks;
using XReports.Table;

namespace XReports.Html.Writers
{
    public interface IHtmlStreamWriter
    {
        Task WriteAsync(IReportTable<HtmlReportCell> reportTable, Stream stream);

        Task WriteAsync(IReportTable<HtmlReportCell> reportTable, StreamWriter streamWriter);
    }
}
