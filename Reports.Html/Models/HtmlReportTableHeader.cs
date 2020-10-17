using System.Collections.Generic;

namespace Reports.Html.Models
{
    public class HtmlReportTableHeader
    {
        public List<List<HtmlReportTableHeaderCell>> Cells { get; set; } = new List<List<HtmlReportTableHeaderCell>>();
    }
}
