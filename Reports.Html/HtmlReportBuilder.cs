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

            foreach (List<IReportCell> row in table.Cells)
            {
                if (row[0].IsHeader)
                {
                    htmlReportTable.Header.Cells.Add(
                        row
                            .Select(cell => new HtmlReportTableHeaderCell()
                            {
                                Text = cell.DisplayValue,
                                ColSpan = cell.ColumnSpan,
                                RowSpan = cell.RowSpan,
                            })
                            .ToList()
                    );
                }
                else
                {
                    htmlReportTable.Body.Cells.Add(
                        row
                            .Select(cell => new HtmlReportTableBodyCell()
                            {
                                Text = cell.DisplayValue,
                                ColSpan = cell.ColumnSpan,
                                RowSpan = cell.RowSpan,
                            })
                            .ToList()
                    );
                }
            }

            return htmlReportTable;
        }
    }
}
