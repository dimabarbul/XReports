using System.Threading.Tasks;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IStringWriter
    {
        Task<string> WriteToStringAsync(IReportTable<HtmlReportCell> reportTable);
        Task WriteToFileAsync(IReportTable<HtmlReportCell> reportTable, string fileName);
    }
}
