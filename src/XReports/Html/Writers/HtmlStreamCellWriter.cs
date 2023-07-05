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
        public Task WriteHeaderCellAsync(StreamWriter streamWriter, HtmlReportCell cell)
        {
            return this.WriteCellAsync(streamWriter, cell, "th");
        }

        /// <inheritdoc />
        public Task WriteBodyCellAsync(StreamWriter streamWriter, HtmlReportCell cell)
        {
            return this.WriteCellAsync(streamWriter, cell, "td");
        }

        /// <summary>
        /// Writes HTML report cell ding stream writer.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <param name="cell">HTML report cell to write.</param>
        /// <param name="tableCellTagName">Name of HTML tag to wrap cell into, e.g., "td" or "th".</param>
        /// <returns>Task.</returns>
        protected virtual async Task WriteCellAsync(StreamWriter streamWriter, HtmlReportCell cell, string tableCellTagName)
        {
            await this.BeginWrappingElementAsync(streamWriter, cell, tableCellTagName).ConfigureAwait(false);
            await this.WriteContentAsync(streamWriter, cell).ConfigureAwait(false);
            await this.EndWrappingElementAsync(streamWriter, tableCellTagName).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes beginning of HTML wrap around cell. By default it is HTML tag for table cell ("td" or "th") with report cell attributes, classes etc.
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <param name="cell">HTML report cell to write.</param>
        /// <param name="tableCellTagName">Name of HTML tag to wrap cell into, e.g., "td" or "th".</param>
        /// <returns>Task.</returns>
        protected virtual async Task BeginWrappingElementAsync(StreamWriter streamWriter, HtmlReportCell cell, string tableCellTagName)
        {
            await streamWriter.WriteAsync('<').ConfigureAwait(false);
            await streamWriter.WriteAsync(tableCellTagName).ConfigureAwait(false);
            await this.WriteAttributesAsync(streamWriter, cell).ConfigureAwait(false);
            await streamWriter.WriteAsync('>').ConfigureAwait(false);
        }

        /// <summary>
        /// Writes ending of HTML wrap around cell. By default it is closing HTML tag for table cell ("td" or "th").
        /// </summary>
        /// <param name="streamWriter">Stream writer to write with.</param>
        /// <param name="tableCellTagName">Name of HTML tag to wrap cell into, e.g., "td" or "th".</param>
        /// <returns>Task.</returns>
        protected virtual async Task EndWrappingElementAsync(StreamWriter streamWriter, string tableCellTagName)
        {
            await streamWriter.WriteAsync("</").ConfigureAwait(false);
            await streamWriter.WriteAsync(tableCellTagName).ConfigureAwait(false);
            await streamWriter.WriteAsync('>').ConfigureAwait(false);
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
