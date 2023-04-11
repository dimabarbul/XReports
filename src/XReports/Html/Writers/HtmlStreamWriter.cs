using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using XReports.Table;

namespace XReports.Html.Writers
{
    /// <summary>
    /// Writer of HTML report to stream.
    /// </summary>
    public class HtmlStreamWriter : IHtmlStreamWriter
    {
        private const int StreamWriterBufferSize = 4096;
        private static readonly Encoding StreamWriterEncoding = Encoding.UTF8;

        private readonly IHtmlStreamCellWriter htmlStreamCellWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlStreamWriter"/> class.
        /// </summary>
        /// <param name="htmlStreamCellWriter">Writer of HTML cell to stream.</param>
        public HtmlStreamWriter(IHtmlStreamCellWriter htmlStreamCellWriter)
        {
            this.htmlStreamCellWriter = htmlStreamCellWriter;
        }

        /// <inheritdoc />
        public async Task WriteAsync(IReportTable<HtmlReportCell> reportTable, Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream, StreamWriterEncoding, StreamWriterBufferSize, true))
            {
                await this.WriteAsync(reportTable, writer).ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public Task WriteAsync(IReportTable<HtmlReportCell> reportTable, StreamWriter streamWriter)
        {
            return this.WriteReportAsync(streamWriter, reportTable);
        }

        /// <summary>
        /// Writes report using stream writer.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <param name="reportTable">Report to write.</param>
        /// <returns>Task.</returns>
        protected virtual async Task WriteReportAsync(StreamWriter streamWriter, IReportTable<HtmlReportCell> reportTable)
        {
            await this.BeginTableAsync(streamWriter).ConfigureAwait(false);
            await this.WriteHeaderAsync(streamWriter, reportTable).ConfigureAwait(false);
            await this.WriteBodyAsync(streamWriter, reportTable).ConfigureAwait(false);
            await this.EndTableAsync(streamWriter).ConfigureAwait(false);

            await streamWriter.FlushAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Writes report header.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <param name="reportTable">Report to write.</param>
        /// <returns>Task.</returns>
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

        /// <summary>
        /// Writes report body.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <param name="reportTable">Report to write.</param>
        /// <returns>Task.</returns>
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

        /// <summary>
        /// Writes opening tag for HTML table header.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <returns>Task.</returns>
        protected virtual Task BeginHeadAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("<thead>");
        }

        /// <summary>
        /// Writes closing tag for HTML table header.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <returns>Task.</returns>
        protected virtual Task EndHeadAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("</thead>");
        }

        /// <summary>
        ///Writes opening tag for HTML table body.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <returns>Task.</returns>
        protected virtual Task BeginBodyAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("<tbody>");
        }

        /// <summary>
        /// Writes closing tag for HTML table body.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <returns>Task.</returns>
        protected virtual Task EndBodyAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("</tbody>");
        }

        /// <summary>
        ///Writes opening tag for HTML table row.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <returns>Task.</returns>
        protected virtual Task BeginRowAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("<tr>");
        }

        /// <summary>
        /// Writes closing tag for HTML table row.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <returns>Task.</returns>
        protected virtual Task EndRowAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("</tr>");
        }

        /// <summary>
        ///Writes opening tag for HTML table.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <returns>Task.</returns>
        protected virtual Task BeginTableAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("<table>");
        }

        /// <summary>
        /// Writes closing tag for HTML table.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <returns>Task.</returns>
        protected virtual Task EndTableAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("</table>");
        }
    }
}
