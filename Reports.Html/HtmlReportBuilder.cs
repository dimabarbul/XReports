using System.Collections.Generic;
using System.Linq;
using Reports.Html.Models;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.Html
{
    public class HtmlReportBuilder
    {
        public HtmlReportTable Build(ReportTable table)
        {
            HtmlReportTable htmlReportTable = new HtmlReportTable();

            this.BuildHeader(table, htmlReportTable);
            this.BuildBody(table, htmlReportTable);

            return htmlReportTable;
        }

        private void BuildHeader(ReportTable table, HtmlReportTable htmlReportTable)
        {
            foreach (IEnumerable<IReportCell> row in table.HeaderCells)
            {
                htmlReportTable.Header.Cells.Add(
                    row
                        .Select(cell => new HtmlReportTableHeaderCell()
                        {
                            Text = cell.DisplayValue,
                            ColSpan = cell.ColumnSpan,
                            RowSpan = cell.RowSpan,
                        })
                );
            }
        }

        private void BuildBody(ReportTable table, HtmlReportTable htmlReportTable)
        {
            foreach (IEnumerable<IReportCell> row in table.Cells)
            {
                htmlReportTable.Body.Cells.Add(
                    row
                        .Select(cell => new HtmlReportTableBodyCell()
                        {
                            Text = cell.DisplayValue,
                            ColSpan = cell.ColumnSpan,
                            RowSpan = cell.RowSpan,
                        })
                );
            }
        }
    }
}
