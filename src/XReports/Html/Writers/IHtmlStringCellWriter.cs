using System.Text;

namespace XReports.Html.Writers
{
    /// <summary>
    /// Interface for writer of cell of HTML report to <see cref="StringBuilder"/>.
    /// </summary>
    public interface IHtmlStringCellWriter
    {
        /// <summary>
        /// Writes header cell to string.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="cell">HTML report cell.</param>
        void WriteHeaderCell(StringBuilder stringBuilder, HtmlReportCell cell);

        /// <summary>
        /// Writes body cell to string.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="cell">HTML report cell.</param>
        void WriteBodyCell(StringBuilder stringBuilder, HtmlReportCell cell);
    }
}
