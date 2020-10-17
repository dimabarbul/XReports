using System.Collections.Generic;

namespace Reports.Html.Models
{
    public class HtmlReportTableHeader
    {
        public ICollection<IEnumerable<HtmlReportTableHeaderCell>> Cells { get; set; } = new List<IEnumerable<HtmlReportTableHeaderCell>>();
    }
}
