using System.Collections.Generic;

namespace Reports.Html.Models
{
    public class HtmlReportTableBody
    {
        public ICollection<IEnumerable<HtmlReportTableBodyCell>> Cells { get; set; } = new List<IEnumerable<HtmlReportTableBodyCell>>();
    }
}
