using System.IO;
using System.Threading.Tasks;

namespace XReports.Html.Writers
{
    /// <summary>
    /// Interface for writer of cell of HTML report to stream.
    /// </summary>
    public interface IHtmlStreamCellWriter
    {
        /// <summary>
        /// Writes header cell to stream.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write cell with.</param>
        /// <param name="cell">HTML report cell.</param>
        /// <returns>Task.</returns>
        Task WriteHeaderCellAsync(StreamWriter streamWriter, HtmlReportCell cell);

        /// <summary>
        /// Writes body cell to stream.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write cell with.</param>
        /// <param name="cell">HTML report cell.</param>
        /// <returns>Task.</returns>
        Task WriteBodyCellAsync(StreamWriter streamWriter, HtmlReportCell cell);
    }
}
