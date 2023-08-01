using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace XReports.Html.Writers
{
    /// <summary>
    /// Writer of HTML report cell to <see cref="StringBuilder"/>.
    /// </summary>
    public class HtmlStringCellWriter : IHtmlStringCellWriter
    {
        /// <inheritdoc />
        public void WriteHeaderCell(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            this.BeginWrappingHeaderElement(stringBuilder, cell);
            this.WriteContent(stringBuilder, cell);
            this.EndWrappingHeaderElement(stringBuilder);
        }

        /// <inheritdoc />
        public void WriteBodyCell(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            this.BeginWrappingElement(stringBuilder, cell);
            this.WriteContent(stringBuilder, cell);
            this.EndWrappingElement(stringBuilder);
        }

        /// <summary>
        /// Writes beginning of HTML wrap around header cell - "th" HTML tag
        /// with report cell attributes, classes etc.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="cell">HTML report cell to write.</param>
        protected virtual void BeginWrappingHeaderElement(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            stringBuilder.Append("<th");
            this.WriteAttributes(stringBuilder, cell);
            stringBuilder.Append('>');
        }

        /// <summary>
        /// Writes beginning of HTML wrap around body cell - "td" HTML tag
        /// with report cell attributes, classes etc.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="cell">HTML report cell to write.</param>
        protected virtual void BeginWrappingElement(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            stringBuilder.Append("<td");
            this.WriteAttributes(stringBuilder, cell);
            stringBuilder.Append('>');
        }

        /// <summary>
        /// Writes ending of HTML wrap around header cell - closing "th" HTML tag.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        protected virtual void EndWrappingHeaderElement(StringBuilder stringBuilder)
        {
            stringBuilder.Append("</th>");
        }

        /// <summary>
        /// Writes ending of HTML wrap around body cell - closing "td" HTML tag.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        protected virtual void EndWrappingElement(StringBuilder stringBuilder)
        {
            stringBuilder.Append("</td>");
        }

        /// <summary>
        /// Writes HTML report cell HTML attributes to string builder.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="cell">HTML report cell to write.</param>
        protected virtual void WriteAttributes(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            if (cell.RowSpan > 1)
            {
                this.WriteAttribute(stringBuilder, "rowSpan", cell.RowSpan.ToString(CultureInfo.InvariantCulture));
            }

            if (cell.ColumnSpan > 1)
            {
                this.WriteAttribute(stringBuilder, "colSpan", cell.ColumnSpan.ToString(CultureInfo.InvariantCulture));
            }

            if (cell.CssClasses.Count > 0)
            {
                this.WriteAttribute(stringBuilder, "class", string.Join(" ", cell.CssClasses));
            }

            if (cell.Styles.Count > 0)
            {
                this.WriteAttribute(
                    stringBuilder,
                    "style",
                    string.Join(
                        " ",
                        cell.Styles
                            .Select(x => $"{x.Key}: {x.Value};")));
            }

            foreach (KeyValuePair<string, string> attribute in cell.Attributes)
            {
                this.WriteAttribute(stringBuilder, attribute.Key, attribute.Value);
            }
        }

        /// <summary>
        /// Writes HTML report cell content to string builder.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="cell">HTML report cell to write.</param>
        protected virtual void WriteContent(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            string value = cell.GetValue<string>();
            if (!cell.IsHtml)
            {
                value = HttpUtility.HtmlEncode(value);
            }

            stringBuilder.Append(value);
        }

        /// <summary>
        /// Writes single attribute to string builder.
        /// </summary>
        /// <param name="stringBuilder">String builder to write to.</param>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        protected virtual void WriteAttribute(StringBuilder stringBuilder, string name, string value)
        {
            stringBuilder.Append(' ').Append(name).Append(@"=""").Append(HttpUtility.HtmlAttributeEncode(value)).Append('"');
        }
    }
}
