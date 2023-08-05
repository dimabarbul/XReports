using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace XReports.Html.Writers
{
    /// <summary>
    /// Writer of HTML report to stream.
    /// </summary>
    public class HtmlStreamCellWriter : IHtmlStreamCellWriter
    {
        /// <inheritdoc />
        public async Task WriteHeaderCellAsync(StreamWriter streamWriter, HtmlReportCell cell)
        {
            await this.BeginWrappingHeaderElementAsync(streamWriter, cell).ConfigureAwait(false);
            await this.WriteContentAsync(streamWriter, cell).ConfigureAwait(false);
            await this.EndWrappingHeaderElementAsync(streamWriter).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task WriteBodyCellAsync(StreamWriter streamWriter, HtmlReportCell cell)
        {
            await this.BeginWrappingElementAsync(streamWriter, cell).ConfigureAwait(false);
            await this.WriteContentAsync(streamWriter, cell).ConfigureAwait(false);
            await this.EndWrappingElementAsync(streamWriter).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes beginning of HTML wrap around header cell - "th" HTML tag
        /// with report cell attributes, classes etc.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <param name="cell">HTML report cell to write.</param>
        /// <returns>Task.</returns>
        protected virtual async Task BeginWrappingHeaderElementAsync(StreamWriter streamWriter, HtmlReportCell cell)
        {
            await streamWriter.WriteAsync("<th").ConfigureAwait(false);
            await this.WriteAttributesAsync(streamWriter, cell).ConfigureAwait(false);
            await streamWriter.WriteAsync('>').ConfigureAwait(false);
        }

        /// <summary>
        /// Writes beginning of HTML wrap around body cell - "td" HTML tag
        /// with report cell attributes, classes etc.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <param name="cell">HTML report cell to write.</param>
        /// <returns>Task.</returns>
        protected virtual async Task BeginWrappingElementAsync(StreamWriter streamWriter, HtmlReportCell cell)
        {
            await streamWriter.WriteAsync("<td").ConfigureAwait(false);
            await this.WriteAttributesAsync(streamWriter, cell).ConfigureAwait(false);
            await streamWriter.WriteAsync('>').ConfigureAwait(false);
        }

        /// <summary>
        /// Writes ending of HTML wrap around header cell - closing "th" HTML tag.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <returns>Task.</returns>
        protected virtual Task EndWrappingHeaderElementAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("</th>");
        }

        /// <summary>
        /// Writes ending of HTML wrap around body cell - closing "td" HTML tag.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <returns>Task.</returns>
        protected virtual Task EndWrappingElementAsync(StreamWriter streamWriter)
        {
            return streamWriter.WriteAsync("</td>");
        }

        /// <summary>
        /// Writes HTML report cell HTML attributes.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <param name="cell">HTML report cell to write.</param>
        /// <returns>Task.</returns>
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

        /// <summary>
        /// Writes HTML report cell content.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <param name="cell">HTML report cell to write.</param>
        /// <returns>Task.</returns>
        protected virtual Task WriteContentAsync(StreamWriter streamWriter, HtmlReportCell cell)
        {
            string value = cell.GetValue<string>();
            if (!cell.IsHtml)
            {
                value = HttpUtility.HtmlEncode(value);
            }

            return streamWriter.WriteAsync(value);
        }

        /// <summary>
        /// Writes single attribute.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        /// <returns>Task.</returns>
        protected virtual async Task WriteAttributeAsync(StreamWriter streamWriter, string name, string value)
        {
            await streamWriter.WriteAsync(' ').ConfigureAwait(false);
            await streamWriter.WriteAsync(name).ConfigureAwait(false);
            await streamWriter.WriteAsync(@"=""").ConfigureAwait(false);
            await streamWriter.WriteAsync(HttpUtility.HtmlAttributeEncode(value)).ConfigureAwait(false);
            await streamWriter.WriteAsync('"').ConfigureAwait(false);
        }
    }
}
