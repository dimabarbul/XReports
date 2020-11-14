using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reports.Html.Models;
using Reports.Interfaces;

namespace Reports.Html.StringWriter
{
    public class StringWriter
    {
        private readonly Func<HtmlReportCell, string> writeHeaderCell;
        private readonly Func<HtmlReportCell, string> writeBodyCell;

        private FileStream fileStream;
        private StringBuilder stringBuilder;
        protected delegate Task WriteTextAsyncDelegate(string text);
        protected WriteTextAsyncDelegate WriteTextAsync;

        public StringWriter()
        {
            GeneralCellWriter cellWriter = new GeneralCellWriter();

            this.writeHeaderCell = cellWriter.WriteHeaderCell;
            this.writeBodyCell = cellWriter.WriteBodyCell;
        }

        public StringWriter(Func<HtmlReportCell, string> writeHeaderCell, Func<HtmlReportCell, string> writeBodyCell)
        {
            this.writeHeaderCell = writeHeaderCell;
            this.writeBodyCell = writeBodyCell;
        }

        public async Task<string> WriteToStringAsync(IReportTable<HtmlReportCell> reportTable)
        {
            this.stringBuilder = new StringBuilder();
            this.WriteTextAsync = this.WriteToStringBuilderAsync;

            await this.WriteReportAsync(reportTable);

            return this.stringBuilder.ToString();
        }

        public async Task WriteToFileAsync(IReportTable<HtmlReportCell> reportTable, string fileName)
        {
            this.fileStream = File.OpenWrite(fileName);
            this.WriteTextAsync = this.WriteToFileAsync;

            await this.WriteReportAsync(reportTable);

            this.fileStream.Close();
        }

        protected virtual async Task WriteReportAsync(IReportTable<HtmlReportCell> reportTable)
        {
            await this.BeginTableAsync();
            await this.WriteHeaderAsync(reportTable);
            await this.WriteBodyAsync(reportTable);
            await this.EndTableAsync();
        }

        protected virtual async Task BeginTableAsync()
        {
            await this.WriteTextAsync("<table>");
        }

        protected virtual async Task WriteHeaderAsync(IReportTable<HtmlReportCell> reportTable)
        {
            bool isHeaderStarted = false;

            foreach (IEnumerable<HtmlReportCell> row in reportTable.HeaderRows)
            {
                if (!isHeaderStarted)
                {
                    await this.WriteTextAsync("<thead>");

                    isHeaderStarted = true;
                }

                await this.WriteTextAsync("<tr>");
                foreach (HtmlReportCell cell in row.Where(c => c != null))
                {
                    await this.WriteTextAsync(this.writeHeaderCell(cell));
                }
                await this.WriteTextAsync("</tr>");
            }

            if (isHeaderStarted)
            {
                await this.WriteTextAsync("</thead>");
            }
        }

        protected virtual async Task WriteBodyAsync(IReportTable<HtmlReportCell> reportTable)
        {
            bool isBodyStarted = false;

            foreach (IEnumerable<HtmlReportCell> row in reportTable.Rows)
            {
                if (!isBodyStarted)
                {
                    await this.WriteTextAsync("<tbody>");

                    isBodyStarted = true;
                }

                await this.WriteTextAsync("<tr>");
                foreach (HtmlReportCell cell in row.Where(c => c != null))
                {
                    await this.WriteTextAsync(this.writeBodyCell(cell));
                }
                await this.WriteTextAsync("</tr>");
            }

            if (isBodyStarted)
            {
                await this.WriteTextAsync("</tbody>");
            }
        }

        protected virtual async Task EndTableAsync()
        {
            await this.WriteTextAsync("</table>");
        }

        private async Task WriteToFileAsync(string text)
        {
            await this.fileStream.WriteAsync(Encoding.UTF8.GetBytes(text));
        }

        private Task WriteToStringBuilderAsync(string text)
        {
            this.stringBuilder.Append(text);

            return Task.CompletedTask;
        }
    }
}
