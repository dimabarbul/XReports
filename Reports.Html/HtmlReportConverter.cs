using System.Collections.Generic;
using System.Linq;
using Reports.Html.Interfaces;
using Reports.Html.Models;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.Html
{
    public class HtmlReportConverter
    {
        private IHtmlPropertyProcessor propertyProcessor;

        public HtmlReportTable Convert(ReportTable table)
        {
            HtmlReportTable htmlReportTable = new HtmlReportTable();

            this.ConvertHeader(table, htmlReportTable);
            this.ConvertBody(table, htmlReportTable);

            return htmlReportTable;
        }

        private void ConvertHeader(ReportTable table, HtmlReportTable htmlReportTable)
        {
            foreach (IEnumerable<IReportCell> row in table.HeaderCells)
            {
                htmlReportTable.Header.Cells.Add(row.Select(this.CreateHeaderHtmlCell));
            }
        }

        private HtmlReportTableHeaderCell CreateHeaderHtmlCell(IReportCell cell)
        {
            HtmlReportTableHeaderCell htmlCell = new HtmlReportTableHeaderCell()
            {
                Text = cell.DisplayValue,
                ColSpan = cell.ColumnSpan,
                RowSpan = cell.RowSpan,
            };

            this.propertyProcessor?.ProcessProperties(cell, htmlCell);

            return htmlCell;
        }

        private void ConvertBody(ReportTable table, HtmlReportTable htmlReportTable)
        {
            foreach (IEnumerable<IReportCell> row in table.Cells)
            {
                htmlReportTable.Body.Cells.Add(row.Select(CreateBodyHtmlCell));
            }
        }

        private HtmlReportTableBodyCell CreateBodyHtmlCell(IReportCell cell)
        {
            HtmlReportTableBodyCell htmlCell = new HtmlReportTableBodyCell()
            {
                Text = cell.DisplayValue,
                ColSpan = cell.ColumnSpan,
                RowSpan = cell.RowSpan,
            };

            this.propertyProcessor?.ProcessProperties(cell, htmlCell);

            return htmlCell;
        }

        public void SetPropertyProcessor(IHtmlPropertyProcessor htmlPropertyProcessor)
        {
            this.propertyProcessor = htmlPropertyProcessor;
        }
    }
}
