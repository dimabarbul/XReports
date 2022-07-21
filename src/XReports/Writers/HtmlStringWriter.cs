using System.Collections.Generic;
using System.Text;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Writers
{
    public class HtmlStringWriter : IHtmlStringWriter
    {
        protected StringBuilder stringBuilder;

        private readonly IHtmlStringCellWriter htmlStringCellWriter;

        public HtmlStringWriter(IHtmlStringCellWriter htmlStringCellWriter)
        {
            this.htmlStringCellWriter = htmlStringCellWriter;
        }

        public string WriteToString(IReportTable<HtmlReportCell> reportTable)
        {
            this.stringBuilder = new StringBuilder();

            this.WriteReport(reportTable);

            return this.stringBuilder.ToString();
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

                    this.htmlStringCellWriter.WriteHeaderCell(this.stringBuilder, cell);
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

                    this.htmlStringCellWriter.WriteBodyCell(this.stringBuilder, cell);
                }

                this.EndRow();
            }

            this.EndBody();
        }

        protected virtual void BeginHead()
        {
            this.stringBuilder.Append("<thead>");
        }

        protected virtual void EndHead()
        {
            this.stringBuilder.Append("</thead>");
        }

        protected virtual void BeginBody()
        {
            this.stringBuilder.Append("<tbody>");
        }

        protected virtual void EndBody()
        {
            this.stringBuilder.Append("</tbody>");
        }

        protected virtual void BeginRow()
        {
            this.stringBuilder.Append("<tr>");
        }

        protected virtual void EndRow()
        {
            this.stringBuilder.Append("</tr>");
        }

        protected virtual void BeginTable()
        {
            this.stringBuilder.Append("<table>");
        }

        protected virtual void EndTable()
        {
            this.stringBuilder.Append("</table>");
        }
    }
}
