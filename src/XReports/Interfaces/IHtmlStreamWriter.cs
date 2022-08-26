using System.IO;
using System.Threading.Tasks;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IHtmlStreamWriter
    {
        Task WriteAsync(IReportTable<HtmlReportCell> reportTable, Stream stream);

        Task WriteAsync(IReportTable<HtmlReportCell> reportTable, StreamWriter streamWriter);
    }
}
