using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace XReports.Html.Writers
{
    public class HtmlStreamCellWriter : IHtmlStreamCellWriter
    {
        public Task WriteHeaderCellAsync(StreamWriter streamWriter, HtmlReportCell cell)
        {
            return this.WriteCellAsync(streamWriter, cell, "th");
        }

        public Task WriteBodyCellAsync(StreamWriter streamWriter, HtmlReportCell cell)
        {
            return this.WriteCellAsync(streamWriter, cell, "td");
        }

        protected virtual async Task WriteCellAsync(StreamWriter streamWriter, HtmlReportCell cell, string tableCellTagName)
        {
            await this.BeginWrappingElementAsync(streamWriter, cell, tableCellTagName).ConfigureAwait(false);
            await this.WriteContentAsync(streamWriter, cell).ConfigureAwait(false);
            await this.EndWrappingElementAsync(streamWriter, tableCellTagName).ConfigureAwait(false);
        }

        protected virtual async Task BeginWrappingElementAsync(StreamWriter streamWriter, HtmlReportCell cell, string tableCellTagName)
        {
            await streamWriter.WriteAsync('<').ConfigureAwait(false);
            await streamWriter.WriteAsync(tableCellTagName).ConfigureAwait(false);
            await streamWriter.WriteAsync(' ').ConfigureAwait(false);
            await this.WriteAttributesAsync(streamWriter, cell).ConfigureAwait(false);
            await streamWriter.WriteAsync('>').ConfigureAwait(false);
        }

        protected virtual async Task EndWrappingElementAsync(StreamWriter streamWriter, string tableCellTagName)
        {
            await streamWriter.WriteAsync("</").ConfigureAwait(false);
            await streamWriter.WriteAsync(tableCellTagName).ConfigureAwait(false);
            await streamWriter.WriteAsync('>').ConfigureAwait(false);
        }

        protected virtual async Task WriteAttributesAsync(StreamWriter streamWriter, HtmlReportCell cell)
        {
            if (cell.RowSpan != 1)
            {
                await this.WriteAttributeAsync(streamWriter, "rowSpan", cell.RowSpan.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            }

            if (cell.ColumnSpan != 1)
            {
                await this.WriteAttributeAsync(streamWriter, "colSpan", cell.ColumnSpan.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            }

            if (cell.CssClasses.Count > 0)
            {
                await this.WriteAttributeAsync(streamWriter, "class", string.Join(" ", cell.CssClasses)).ConfigureAwait(false);
            }

            if (cell.Styles.Count > 0)
            {
                await this.WriteAttributeAsync(
                    streamWriter,
                    "style",
                    string.Join(
                        " ",
                        cell.Styles
                            .Select(x => $"{x.Key}: {x.Value};"))).ConfigureAwait(false);
            }

            foreach (KeyValuePair<string, string> attribute in cell.Attributes)
            {
                await this.WriteAttributeAsync(streamWriter, attribute.Key, attribute.Value).ConfigureAwait(false);
            }
        }

        protected virtual Task WriteContentAsync(StreamWriter streamWriter, HtmlReportCell cell)
        {
            string value = cell.GetValue<string>();
            if (!cell.IsHtml)
            {
                value = HttpUtility.HtmlEncode(value);
            }

            return streamWriter.WriteAsync(value);
        }

        protected virtual async Task WriteAttributeAsync(StreamWriter streamWriter, string name, string value)
        {
            await streamWriter.WriteAsync(name).ConfigureAwait(false);
            await streamWriter.WriteAsync(@"=""").ConfigureAwait(false);
            await streamWriter.WriteAsync(HttpUtility.HtmlAttributeEncode(value)).ConfigureAwait(false);
            await streamWriter.WriteAsync(@""" ").ConfigureAwait(false);
        }
    }
}
