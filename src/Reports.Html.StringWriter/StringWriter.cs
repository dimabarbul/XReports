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

        public StringWriter(Func<HtmlReportCell, string> writeHeaderCell, Func<HtmlReportCell, string> writeBodyCell)
        {
            this.writeHeaderCell = writeHeaderCell;
            this.writeBodyCell = writeBodyCell;
        }

        public async Task WriteToFileAsync(IReportTable<HtmlReportCell> reportTable, string fileName)
        {
            FileStream fileStream = File.OpenWrite(fileName);

            await this.StartWritingAsync(fileStream);
            await this.WriteHeader(reportTable, fileStream);
            await this.WriteBody(reportTable, fileStream);
            await this.EndWriting(fileStream);

            fileStream.Close();
        }

        private async Task StartWritingAsync(FileStream fileStream)
        {
            await this.WriteTextAsync(fileStream, "<table>");
        }

        private async Task WriteHeader(IReportTable<HtmlReportCell> reportTable, FileStream fileStream)
        {
            bool isHeaderStarted = false;

            foreach (IEnumerable<HtmlReportCell> row in reportTable.HeaderRows)
            {
                if (!isHeaderStarted)
                {
                    await this.WriteTextAsync(fileStream, "<thead>");

                    isHeaderStarted = true;
                }

                await this.WriteTextAsync(fileStream, "<tr>");
                foreach (HtmlReportCell cell in row.Where(c => c != null))
                {
                    await this.WriteTextAsync(fileStream, this.writeHeaderCell(cell));
                }
                await this.WriteTextAsync(fileStream, "</tr>");
            }

            if (isHeaderStarted)
            {
                await this.WriteTextAsync(fileStream, "</thead>");
            }
        }

        private async Task WriteBody(IReportTable<HtmlReportCell> reportTable, FileStream fileStream)
        {
            bool isBodyStarted = false;

            foreach (IEnumerable<HtmlReportCell> row in reportTable.Rows)
            {
                if (!isBodyStarted)
                {
                    await this.WriteTextAsync(fileStream, "<tbody>");

                    isBodyStarted = true;
                }

                await this.WriteTextAsync(fileStream, "<tr>");
                foreach (HtmlReportCell cell in row.Where(c => c != null))
                {
                    await this.WriteTextAsync(fileStream, this.writeBodyCell(cell));
                }
                await this.WriteTextAsync(fileStream, "</tr>");
            }

            if (isBodyStarted)
            {
                await this.WriteTextAsync(fileStream, "</tbody>");
            }
        }

        private async Task EndWriting(FileStream fileStream)
        {
            await this.WriteTextAsync(fileStream, "</table>");
        }

        private async Task WriteTextAsync(Stream stream, string text)
        {
            await stream.WriteAsync(Encoding.UTF8.GetBytes(text));
        }
    }
}
