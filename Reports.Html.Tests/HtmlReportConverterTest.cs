using System.Collections.Generic;
using System.Linq;
using Reports.Html.Models;
using Reports.Interfaces;

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

        private class ReportTable : IReportTable
        {
            public IEnumerable<IEnumerable<IReportCell>> HeaderRows { get; }
            public IEnumerable<IEnumerable<IReportCell>> Rows { get; }

            public ReportTable(IEnumerable<IEnumerable<IReportCell>> headerRows, IEnumerable<IEnumerable<IReportCell>> rows)
            {
                this.HeaderRows = headerRows;
                this.Rows = rows;
            }
        }
    }
}
