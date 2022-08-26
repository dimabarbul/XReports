using System.Collections.Generic;
using System.Text;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Writers
{
    public class HtmlStringWriter : IHtmlStringWriter
    {
        private readonly IHtmlStringCellWriter htmlStringCellWriter;

        public HtmlStringWriter(IHtmlStringCellWriter htmlStringCellWriter)
        {
            this.htmlStringCellWriter = htmlStringCellWriter;
        }

        protected StringBuilder StringBuilder { get; private set; }

        public string WriteToString(IReportTable<HtmlReportCell> reportTable)
        {
            this.StringBuilder = new StringBuilder();

            this.WriteReport(reportTable);

            return this.StringBuilder.ToString();
        }

        protected virtual void WriteReport(IReportTable<HtmlReportCell> reportTable)
        {
            this.BeginTable();
            this.WriteHeader(reportTable);
            this.WriteBody(reportTable);
            this.EndTable();
        }

        protected virtual void WriteHeader(IReportTable<HtmlReportCell> reportTable)
        {
            this.BeginHead();
            foreach (IEnumerable<HtmlReportCell> row in reportTable.HeaderRows)
            {
                this.BeginRow();

                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    this.htmlStringCellWriter.WriteHeaderCell(this.StringBuilder, cell);
                }

                this.EndRow();
            }

            this.EndHead();
        }

        protected virtual void WriteBody(IReportTable<HtmlReportCell> reportTable)
        {
            this.BeginBody();
            foreach (IEnumerable<HtmlReportCell> row in reportTable.Rows)
            {
                this.BeginRow();

                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    this.htmlStringCellWriter.WriteBodyCell(this.StringBuilder, cell);
                }

                this.EndRow();
            }

            this.EndBody();
        }

        protected virtual void BeginHead()
        {
            this.StringBuilder.Append("<thead>");
        }

        protected virtual void EndHead()
        {
            this.StringBuilder.Append("</thead>");
        }

        protected virtual void BeginBody()
        {
            this.StringBuilder.Append("<tbody>");
        }

        protected virtual void EndBody()
        {
            this.StringBuilder.Append("</tbody>");
        }

        protected virtual void BeginRow()
        {
            this.StringBuilder.Append("<tr>");
        }

        protected virtual void EndRow()
        {
            this.StringBuilder.Append("</tr>");
        }

        protected virtual void BeginTable()
        {
            this.StringBuilder.Append("<table>");
        }

        protected virtual void EndTable()
        {
            this.StringBuilder.Append("</table>");
        }
    }
}
