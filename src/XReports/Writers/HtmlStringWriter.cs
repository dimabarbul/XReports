using System.Collections.Generic;
using System.Text;
using XReports.Interfaces;
using XReports.Models;
using XReports.Table;

namespace XReports.Writers
{
    public class HtmlStringWriter : IHtmlStringWriter
    {
        private readonly IHtmlStringCellWriter htmlStringCellWriter;

        public HtmlStringWriter(IHtmlStringCellWriter htmlStringCellWriter)
        {
            this.htmlStringCellWriter = htmlStringCellWriter;
        }

        public string WriteToString(IReportTable<HtmlReportCell> reportTable)
        {
            StringBuilder stringBuilder = new StringBuilder();

            this.WriteReport(stringBuilder, reportTable);

            return stringBuilder.ToString();
        }

        protected virtual void WriteReport(StringBuilder stringBuilder, IReportTable<HtmlReportCell> reportTable)
        {
            this.BeginTable(stringBuilder);
            this.WriteHeader(stringBuilder, reportTable);
            this.WriteBody(stringBuilder, reportTable);
            this.EndTable(stringBuilder);
        }

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

        protected virtual void BeginHead(StringBuilder stringBuilder)
        {
            stringBuilder.Append("<thead>");
        }

        protected virtual void EndHead(StringBuilder stringBuilder)
        {
            stringBuilder.Append("</thead>");
        }

        protected virtual void BeginBody(StringBuilder stringBuilder)
        {
            stringBuilder.Append("<tbody>");
        }

        protected virtual void EndBody(StringBuilder stringBuilder)
        {
            stringBuilder.Append("</tbody>");
        }

        protected virtual void BeginRow(StringBuilder stringBuilder)
        {
            stringBuilder.Append("<tr>");
        }

        protected virtual void EndRow(StringBuilder stringBuilder)
        {
            stringBuilder.Append("</tr>");
        }

        protected virtual void BeginTable(StringBuilder stringBuilder)
        {
            stringBuilder.Append("<table>");
        }

        protected virtual void EndTable(StringBuilder stringBuilder)
        {
            stringBuilder.Append("</table>");
        }
    }
}
