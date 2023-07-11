using XReports.Table;

namespace XReports.Html.Writers
{
    /// <summary>
    /// Interface for writer of HTML report to string.
    /// </summary>
    public interface IHtmlStringWriter
    {
        /// <summary>
        /// Writes HTML report to string.
        /// </summary>
        /// <param name="reportTable">Report to write.</param>
        /// <returns>HTML representation of the report.</returns>
        string Write(IReportTable<HtmlReportCell> reportTable);
    }
}
