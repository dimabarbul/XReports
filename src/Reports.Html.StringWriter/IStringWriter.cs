using System.Threading.Tasks;
using Reports.Core.Interfaces;
using Reports.Core.Models;

namespace Reports.Html.StringWriter
{
    public interface IStringWriter
    {
        Task<string> WriteToStringAsync(IReportTable<HtmlReportCell> reportTable);
        Task WriteToFileAsync(IReportTable<HtmlReportCell> reportTable, string fileName);
    }
}
