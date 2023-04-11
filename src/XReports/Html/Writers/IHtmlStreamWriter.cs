using System.IO;
using System.Threading.Tasks;
using XReports.Table;

namespace XReports.Html.Writers
{
    /// <summary>
    /// Interface for writer of HTML report to stream.
    /// </summary>
    public interface IHtmlStreamWriter
    {
        /// <summary>
        /// Writes HTML report to stream.
        /// </summary>
        /// <param name="reportTable">Report to write.</param>
        /// <param name="stream">Stream to write report to. Stream will be rewound to beginning.</param>
        /// <returns>Task.</returns>
        Task WriteAsync(IReportTable<HtmlReportCell> reportTable, Stream stream);

        /// <summary>
        /// Writes HTML report using stream writer.
        /// </summary>
        /// <param name="reportTable">Report to write.</param>
        /// <param name="streamWriter">Stream writer to write report with.</param>
        /// <returns>Task.</returns>
        Task WriteAsync(IReportTable<HtmlReportCell> reportTable, StreamWriter streamWriter);
    }
}
