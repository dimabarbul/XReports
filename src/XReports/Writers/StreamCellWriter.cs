using System.Linq;
using System.Threading.Tasks;
using System.Web;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Writers
{
    public class StreamCellWriter : IStreamCellWriter
    {
        public Task WriteHeaderCellAsync(System.IO.StreamWriter streamWriter, HtmlReportCell cell)
        {
            return this.WriteCellAsync(streamWriter, cell, "th");
        }

        public Task WriteBodyCellAsync(System.IO.StreamWriter streamWriter, HtmlReportCell cell)
        {
            return this.WriteCellAsync(streamWriter, cell, "td");
        }

        protected virtual async Task WriteCellAsync(System.IO.StreamWriter streamWriter, HtmlReportCell cell, string tableCellTagName)
        {
            await this.BeginWrappingElementAsync(streamWriter, cell, tableCellTagName);
            await this.WriteContentAsync(streamWriter, cell);
            await this.EndWrappingElementAsync(streamWriter, tableCellTagName);
        }

        protected virtual async Task BeginWrappingElementAsync(System.IO.StreamWriter streamWriter, HtmlReportCell cell, string tableCellTagName)
        {
            await streamWriter.WriteAsync("<");
            await streamWriter.WriteAsync(tableCellTagName);
            await streamWriter.WriteAsync(" ");
            await this.WriteAttributesAsync(streamWriter, cell);
            await streamWriter.WriteAsync(">");
        }

        protected virtual async Task EndWrappingElementAsync(System.IO.StreamWriter streamWriter, string tableCellTagName)
        {
            await streamWriter.WriteAsync("</");
            await streamWriter.WriteAsync(tableCellTagName);
            await streamWriter.WriteAsync(">");
        }

        protected async Task WriteAttributesAsync(System.IO.StreamWriter streamWriter, HtmlReportCell cell)
        {
            if (cell.RowSpan != 1)
            {
                await this.WriteAttributeAsync(streamWriter, "rowSpan", cell.RowSpan.ToString());
            }

            if (cell.ColumnSpan != 1)
            {
                await this.WriteAttributeAsync(streamWriter, "colSpan", cell.ColumnSpan.ToString());
            }

            if (cell.CssClasses.Count > 0)
            {
                await this.WriteAttributeAsync(streamWriter, "class", string.Join(" ", cell.CssClasses));
            }

            if (cell.Styles.Count > 0)
            {
                await this.WriteAttributeAsync(
                    streamWriter,
                    "style",
                    string.Join(
                        " ",
                        cell.Styles
                            .Select(x => $"{x.Key}: {x.Value};")));
            }

            foreach ((string name, string value) in cell.Attributes)
            {
                await this.WriteAttributeAsync(streamWriter, name, value);
            }
        }

        protected Task WriteContentAsync(System.IO.StreamWriter streamWriter, HtmlReportCell cell)
        {
            string value = cell.GetValue<string>();
            if (!cell.IsHtml)
            {
                value = HttpUtility.HtmlEncode(value);
            }

            return streamWriter.WriteAsync(value);
        }

        protected async Task WriteAttributeAsync(System.IO.StreamWriter streamWriter, string name, string value)
        {
            await streamWriter.WriteAsync(name);
            await streamWriter.WriteAsync(@"=""");
            await streamWriter.WriteAsync(HttpUtility.HtmlAttributeEncode(value));
            await streamWriter.WriteAsync(@""" ");
        }
    }
}
