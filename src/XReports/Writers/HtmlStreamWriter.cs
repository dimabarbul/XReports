using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Writers
{
    public class HtmlStreamWriter : IHtmlStreamWriter
    {
        private readonly IHtmlStreamCellWriter htmlStreamCellWriter;
        private StreamWriter streamWriter;

        public HtmlStreamWriter(IHtmlStreamCellWriter htmlStreamCellWriter)
        {
            this.htmlStreamCellWriter = htmlStreamCellWriter;
        }

        public async Task WriteAsync(IReportTable<HtmlReportCell> reportTable, Stream stream)
        {
            await using StreamWriter writer = new StreamWriter(stream, leaveOpen: true);
            await this.WriteAsync(reportTable, writer).ConfigureAwait(false);
        }

        public Task WriteAsync(IReportTable<HtmlReportCell> reportTable, StreamWriter streamWriter)
        {
            this.streamWriter = streamWriter;

            return this.WriteReportAsync(reportTable);
        }

        protected virtual async Task WriteReportAsync(IReportTable<HtmlReportCell> reportTable)
        {
            await this.BeginTableAsync().ConfigureAwait(false);
            await this.WriteHeaderAsync(reportTable).ConfigureAwait(false);
            await this.WriteBodyAsync(reportTable).ConfigureAwait(false);
            await this.EndTableAsync().ConfigureAwait(false);

            await this.streamWriter.FlushAsync().ConfigureAwait(false);
        }

        protected virtual async Task WriteHeaderAsync(IReportTable<HtmlReportCell> reportTable)
        {
            await this.BeginHeadAsync().ConfigureAwait(false);

            foreach (IEnumerable<HtmlReportCell> row in reportTable.HeaderRows)
            {
                await this.BeginRowAsync().ConfigureAwait(false);

                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    await this.htmlStreamCellWriter.WriteHeaderCellAsync(this.streamWriter, cell).ConfigureAwait(false);
                }

                await this.EndRowAsync().ConfigureAwait(false);
            }

            await this.EndHeadAsync().ConfigureAwait(false);
        }

        protected virtual async Task WriteBodyAsync(IReportTable<HtmlReportCell> reportTable)
        {
            await this.BeginBodyAsync().ConfigureAwait(false);

            foreach (IEnumerable<HtmlReportCell> row in reportTable.Rows)
            {
                await this.BeginRowAsync().ConfigureAwait(false);

                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    await this.htmlStreamCellWriter.WriteBodyCellAsync(this.streamWriter, cell).ConfigureAwait(false);
                }

                await this.EndRowAsync().ConfigureAwait(false);
            }

            await this.EndBodyAsync().ConfigureAwait(false);
        }

        protected virtual Task BeginHeadAsync()
        {
            return this.streamWriter.WriteAsync("<thead>");
        }

        protected virtual Task EndHeadAsync()
        {
            return this.streamWriter.WriteAsync("</thead>");
        }

        protected virtual Task BeginBodyAsync()
        {
            return this.streamWriter.WriteAsync("<tbody>");
        }

        protected virtual Task EndBodyAsync()
        {
            return this.streamWriter.WriteAsync("</tbody>");
        }

        protected virtual Task BeginRowAsync()
        {
            return this.streamWriter.WriteAsync("<tr>");
        }

        protected virtual Task EndRowAsync()
        {
            return this.streamWriter.WriteAsync("</tr>");
        }

        protected virtual Task BeginTableAsync()
        {
            return this.streamWriter.WriteAsync("<table>");
        }

        protected virtual Task EndTableAsync()
        {
            return this.streamWriter.WriteAsync("</table>");
        }
    }
}
