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
        private delegate Task WriteTextAsyncDelegate(string text);
        private WriteTextAsyncDelegate writeTextAsync;

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

        public async Task<string> WriteToString(IReportTable<HtmlReportCell> reportTable)
        {
            this.stringBuilder = new StringBuilder();
            this.writeTextAsync = this.WriteToStringBuilderAsync;

            await this.WriteReportAsync(reportTable);

            return this.stringBuilder.ToString();
        }

        public async Task WriteToFileAsync(IReportTable<HtmlReportCell> reportTable, string fileName)
        {
            this.fileStream = File.OpenWrite(fileName);
            this.writeTextAsync = this.WriteToFileAsync;

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
            await this.writeTextAsync("<table>");
        }

        protected virtual async Task WriteHeaderAsync(IReportTable<HtmlReportCell> reportTable)
        {
            bool isHeaderStarted = false;

            foreach (IEnumerable<HtmlReportCell> row in reportTable.HeaderRows)
            {
                if (!isHeaderStarted)
                {
                    await this.writeTextAsync("<thead>");

                    isHeaderStarted = true;
                }

                await this.writeTextAsync("<tr>");
                foreach (HtmlReportCell cell in row.Where(c => c != null))
                {
                    await this.writeTextAsync(this.writeHeaderCell(cell));
                }
                await this.writeTextAsync("</tr>");
            }

            if (isHeaderStarted)
            {
                await this.writeTextAsync("</thead>");
            }
        }

        protected virtual async Task WriteBodyAsync(IReportTable<HtmlReportCell> reportTable)
        {
            bool isBodyStarted = false;

            foreach (IEnumerable<HtmlReportCell> row in reportTable.Rows)
            {
                if (!isBodyStarted)
                {
                    await this.writeTextAsync("<tbody>");

                    isBodyStarted = true;
                }

                await this.writeTextAsync("<tr>");
                foreach (HtmlReportCell cell in row.Where(c => c != null))
                {
                    await this.writeTextAsync(this.writeBodyCell(cell));
                }
                await this.writeTextAsync("</tr>");
            }

            if (isBodyStarted)
            {
                await this.writeTextAsync("</tbody>");
            }
        }

        protected virtual async Task EndTableAsync()
        {
            await this.writeTextAsync("</table>");
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
