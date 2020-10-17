using System.Collections.Generic;

namespace Reports.Html.Models
{
    public class HtmlReportTableBody
    {
        public List<List<HtmlReportTableBodyCell>> Cells { get; set; } = new List<List<HtmlReportTableBodyCell>>();
    }
}
