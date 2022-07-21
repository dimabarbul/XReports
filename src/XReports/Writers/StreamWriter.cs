using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Writers
{
    public class StreamWriter : IStreamWriter
    {
        private readonly IStreamCellWriter streamCellWriter;
        private System.IO.StreamWriter streamWriter;

        public StreamWriter(IStreamCellWriter streamCellWriter)
        {
            this.streamCellWriter = streamCellWriter;
        }

        public async Task WriteAsync(IReportTable<HtmlReportCell> reportTable, Stream stream)
        {
            await using System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, leaveOpen: true);
            await this.WriteAsync(reportTable, writer);
        }

        public Task WriteAsync(IReportTable<HtmlReportCell> reportTable, System.IO.StreamWriter streamWriter)
        {
            this.streamWriter = streamWriter;

            return this.WriteReportAsync(reportTable);
        }

        protected virtual async Task WriteReportAsync(IReportTable<HtmlReportCell> reportTable)
        {
            await this.BeginTableAsync();
            await this.WriteHeaderAsync(reportTable);
            await this.WriteBodyAsync(reportTable);
            await this.EndTableAsync();

            await this.streamWriter.FlushAsync();
        }

        protected virtual async Task WriteHeaderAsync(IReportTable<HtmlReportCell> reportTable)
        {
            await this.BeginHeadAsync();

            foreach (IEnumerable<HtmlReportCell> row in reportTable.HeaderRows)
            {
                await this.BeginRowAsync();

                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    await this.streamCellWriter.WriteHeaderCellAsync(this.streamWriter, cell);
                }

                await this.EndRowAsync();
            }

            await this.EndHeadAsync();
        }

        protected virtual async Task WriteBodyAsync(IReportTable<HtmlReportCell> reportTable)
        {
            await this.BeginBodyAsync();

            foreach (IEnumerable<HtmlReportCell> row in reportTable.Rows)
            {
                await this.BeginRowAsync();

                foreach (HtmlReportCell cell in row)
                {
                    if (cell == null)
                    {
                        continue;
                    }

                    await this.streamCellWriter.WriteBodyCellAsync(this.streamWriter, cell);
                }

                await this.EndRowAsync();
            }

            await this.EndBodyAsync();
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
