using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using XReports.Interfaces;
using XReports.Models;
using XReports.Table;

namespace XReports.Writers
{
    public class HtmlStreamWriter : IHtmlStreamWriter
    {
        private const int StreamWriterBufferSize = 4096;
        private static readonly Encoding StreamWriterEncoding = Encoding.UTF8;

        private readonly IHtmlStreamCellWriter htmlStreamCellWriter;

        public HtmlStreamWriter(IHtmlStreamCellWriter htmlStreamCellWriter)
        {
            this.htmlStreamCellWriter = htmlStreamCellWriter;
        }

        public async Task WriteAsync(IReportTable<HtmlReportCell> reportTable, Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream, StreamWriterEncoding, StreamWriterBufferSize, true))
            {
                await this.WriteAsync(reportTable, writer).ConfigureAwait(false);
            }
        }

        public Task WriteAsync(IReportTable<HtmlReportCell> reportTable, StreamWriter streamWriter)
        {
            return this.WriteReportAsync(reportTable, streamWriter);
        }

        protected virtual async Task WriteReportAsync(IReportTable<HtmlReportCell> reportTable, StreamWriter streamWriter)
        {
            await this.BeginTableAsync(streamWriter).ConfigureAwait(false);
            await this.WriteHeaderAsync(streamWriter, reportTable).ConfigureAwait(false);
            await this.WriteBodyAsync(streamWriter, reportTable).ConfigureAwait(false);
            await this.EndTableAsync(streamWriter).ConfigureAwait(false);

            await streamWriter.FlushAsync().ConfigureAwait(false);
        }

        protected virtual async Task WriteHeaderAsync(StreamWriter streamWriter, IReportTable<HtmlReportCell> reportTable)
        {
            await this.BeginHeadAsync(streamWriter).ConfigureAwait(false);

            foreach (IEnumerable<HtmlReportCell> row in reportTable.HeaderRows)
            {
                await this.BeginRowAsync(streamWriter).ConfigureAwait(false);

                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    await this.htmlStreamCellWriter.WriteHeaderCellAsync(streamWriter, cell).ConfigureAwait(false);
                }

                await this.EndRowAsync(streamWriter).ConfigureAwait(false);
            }

            await this.EndHeadAsync(streamWriter).ConfigureAwait(false);
        }

        protected virtual async Task WriteBodyAsync(StreamWriter streamWriter, IReportTable<HtmlReportCell> reportTable)
        {
            await this.BeginBodyAsync(streamWriter).ConfigureAwait(false);

            foreach (IEnumerable<HtmlReportCell> row in reportTable.Rows)
            {
                await this.BeginRowAsync(streamWriter).ConfigureAwait(false);

                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    await this.htmlStreamCellWriter.WriteBodyCellAsync(streamWriter, cell).ConfigureAwait(false);
                }

                await this.EndRowAsync(streamWriter).ConfigureAwait(false);
            }

            await this.EndBodyAsync(streamWriter).ConfigureAwait(false);
        }

        protected virtual Task BeginHeadAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("<thead>");
        }

        protected virtual Task EndHeadAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("</thead>");
        }

        protected virtual Task BeginBodyAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("<tbody>");
        }

        protected virtual Task EndBodyAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("</tbody>");
        }

        protected virtual Task BeginRowAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("<tr>");
        }

        protected virtual Task EndRowAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("</tr>");
        }

        protected virtual Task BeginTableAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("<table>");
        }

        protected virtual Task EndTableAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("</table>");
        }
    }
}
