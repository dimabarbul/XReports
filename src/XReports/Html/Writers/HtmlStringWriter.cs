using System.Collections.Generic;
using System.Text;
using XReports.Table;

namespace XReports.Html.Writers
{
    /// <summary>
    /// Writer of HTML report to string using <see cref="StringBuilder"/>.
    /// </summary>
    public class HtmlStringWriter : IHtmlStringWriter
    {
        private readonly IHtmlStringCellWriter htmlStringCellWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlStringWriter"/> class.
        /// </summary>
        /// <param name="htmlStringCellWriter">Writer of HTML cell to string.</param>
        public HtmlStringWriter(IHtmlStringCellWriter htmlStringCellWriter)
        {
            this.htmlStringCellWriter = htmlStringCellWriter;
        }

        /// <inheritdoc />
        public string Write(IReportTable<HtmlReportCell> reportTable)
        {
            StringBuilder stringBuilder = new StringBuilder();

            this.WriteReport(stringBuilder, reportTable);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Writes report to string builder.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="reportTable">Report to write.</param>
        protected virtual void WriteReport(StringBuilder stringBuilder, IReportTable<HtmlReportCell> reportTable)
        {
            this.BeginTable(stringBuilder, reportTable);
            this.WriteHeader(stringBuilder, reportTable);
            this.WriteBody(stringBuilder, reportTable);
            this.EndTable(stringBuilder, reportTable);
        }

        /// <summary>
        /// Writes report header.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="reportTable">Report to write.</param>
        protected virtual void WriteHeader(StringBuilder stringBuilder, IReportTable<HtmlReportCell> reportTable)
        {
            this.BeginHead(stringBuilder);
            foreach (IEnumerable<HtmlReportCell> row in reportTable.HeaderRows)
            {
                this.BeginRow(stringBuilder);

                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    this.htmlStringCellWriter.WriteHeaderCell(stringBuilder, cell);
                }

                this.EndRow(stringBuilder);
            }

            this.EndHead(stringBuilder);
        }

        /// <summary>
        /// Writes report body.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="reportTable">Report to write.</param>
        protected virtual void WriteBody(StringBuilder stringBuilder, IReportTable<HtmlReportCell> reportTable)
        {
            this.BeginBody(stringBuilder);
            foreach (IEnumerable<HtmlReportCell> row in reportTable.Rows)
            {
                this.BeginRow(stringBuilder);

                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    this.htmlStringCellWriter.WriteBodyCell(stringBuilder, cell);
                }

                this.EndRow(stringBuilder);
            }

            this.EndBody(stringBuilder);
        }

        /// <summary>
        /// Writes opening tag for HTML table header.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        protected virtual void BeginHead(StringBuilder stringBuilder)
        {
            stringBuilder.Append("<thead>");
        }

        /// <summary>
        /// Writes closing tag for HTML table header.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        protected virtual void EndHead(StringBuilder stringBuilder)
        {
            stringBuilder.Append("</thead>");
        }

        /// <summary>
        ///Writes opening tag for HTML table body.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        protected virtual void BeginBody(StringBuilder stringBuilder)
        {
            stringBuilder.Append("<tbody>");
        }

        /// <summary>
        /// Writes closing tag for HTML table body.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        protected virtual void EndBody(StringBuilder stringBuilder)
        {
            stringBuilder.Append("</tbody>");
        }

        /// <summary>
        ///Writes opening tag for HTML table row.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        protected virtual void BeginRow(StringBuilder stringBuilder)
        {
            stringBuilder.Append("<tr>");
        }

        /// <summary>
        /// Writes closing tag for HTML table row.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        protected virtual void EndRow(StringBuilder stringBuilder)
        {
            stringBuilder.Append("</tr>");
        }

        /// <summary>
        /// Writes opening tag for HTML table.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="reportTable">Report to write.</param>
        protected virtual void BeginTable(StringBuilder stringBuilder, IReportTable<HtmlReportCell> reportTable)
        {
            stringBuilder.Append("<table>");
        }

        /// <summary>
        /// Writes closing tag for HTML table.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="reportTable">Report to write.</param>
        protected virtual void EndTable(StringBuilder stringBuilder, IReportTable<HtmlReportCell> reportTable)
        {
            stringBuilder.Append("</table>");
        }
    }
}
