namespace Reports.Html.Models
{
    public class HtmlReportTable
    {
        public HtmlReportTableHeader Header { get; set; } = new HtmlReportTableHeader();
        public HtmlReportTableBody Body { get; set; } = new HtmlReportTableBody();
    }
}
