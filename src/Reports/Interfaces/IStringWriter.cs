using System.Threading.Tasks;
using Reports.Models;

namespace Reports.Interfaces
{
    public interface IStringWriter
    {
        Task<string> WriteToStringAsync(IReportTable<HtmlReportCell> reportTable);
        Task WriteToFileAsync(IReportTable<HtmlReportCell> reportTable, string fileName);
    }
}
