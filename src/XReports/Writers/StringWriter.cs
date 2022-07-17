using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Writers
{
    public class StringWriter : IStringWriter
    {
        private readonly IStringCellWriter stringCellWriter;
        private FileStream fileStream;
        private StringBuilder stringBuilder;
        private WriteTextAsyncDelegate writeTextAsync;

        public StringWriter(IStringCellWriter stringCellWriter)
        {
            this.stringCellWriter = stringCellWriter;
        }

        private delegate Task WriteTextAsyncDelegate(string text);

        public async Task<string> WriteToStringAsync(IReportTable<HtmlReportCell> reportTable)
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

        protected virtual async Task WriteHeaderAsync(IReportTable<HtmlReportCell> reportTable)
        {
            bool isHeaderStarted = false;

            foreach (IEnumerable<HtmlReportCell> row in reportTable.HeaderRows)
            {
                if (!isHeaderStarted)
                {
                    await this.BeginHeadAsync();

                    isHeaderStarted = true;
                }

                await this.BeginRowAsync();
                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    await this.WriteTextAsync(this.stringCellWriter.WriteHeaderCell(cell));
                }

                await this.EndRowAsync();
            }

            if (isHeaderStarted)
            {
                await this.EndHeadAsync();
            }
        }

        protected virtual async Task WriteBodyAsync(IReportTable<HtmlReportCell> reportTable)
        {
            bool isBodyStarted = false;

            foreach (IEnumerable<HtmlReportCell> row in reportTable.Rows)
            {
                if (!isBodyStarted)
                {
                    await this.BeginBodyAsync();

                    isBodyStarted = true;
                }

                await this.BeginRowAsync();
                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    await this.WriteTextAsync(this.stringCellWriter.WriteBodyCell(cell));
                }

                await this.EndRowAsync();
            }

            if (isBodyStarted)
            {
                await this.EndBodyAsync();
            }
        }

        protected virtual async Task BeginHeadAsync()
        {
            await this.WriteTextAsync("<thead>");
        }

        protected virtual async Task EndHeadAsync()
        {
            await this.WriteTextAsync("</thead>");
        }

        protected virtual async Task BeginBodyAsync()
        {
            await this.WriteTextAsync("<tbody>");
        }

        protected virtual async Task EndBodyAsync()
        {
            await this.WriteTextAsync("</tbody>");
        }

        protected virtual async Task BeginRowAsync()
        {
            await this.WriteTextAsync("<tr>");
        }

        protected virtual async Task EndRowAsync()
        {
            await this.WriteTextAsync("</tr>");
        }

        protected virtual async Task BeginTableAsync()
        {
            await this.WriteTextAsync("<table>");
        }

        protected virtual async Task EndTableAsync()
        {
            await this.WriteTextAsync("</table>");
        }

        protected Task WriteTextAsync(string text)
        {
            return this.writeTextAsync(text);
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
