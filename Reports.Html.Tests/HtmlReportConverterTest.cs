using System.Linq;
using Reports.Html.Models;

namespace Reports.Html.Tests
{
    public partial class HtmlReportConverterTest
    {
        private HtmlReportTableHeaderCell[][] GetHeaderCellsAsArray(HtmlReportTable table)
        {
            return table.Header.Cells.Select(row => row.ToArray()).ToArray();
        }

        private HtmlReportTableBodyCell[][] GetBodyCellsAsArray(HtmlReportTable table)
        {
            return table.Body.Cells.Select(row => row.ToArray()).ToArray();
        }
    }
}
