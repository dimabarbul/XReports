using System;
using System.Linq;
using System.Text;
using System.Web;
using Reports.Models;

namespace Reports.Html.StringWriter
{
    public class StringCellWriter
    {
        public string WriteHeaderCell(HtmlReportCell cell)
        {
            StringBuilder stringBuilder = new StringBuilder();

            this.WriteCell(stringBuilder, cell, "th");

            return stringBuilder.ToString();
        }

        public string WriteBodyCell(HtmlReportCell cell)
        {
            StringBuilder stringBuilder = new StringBuilder();

            this.WriteCell(stringBuilder, cell, "td");

            return stringBuilder.ToString();
        }

        protected virtual void WriteCell(StringBuilder stringBuilder, HtmlReportCell cell, string tableCellTagName)
        {
            stringBuilder.Append("<").Append(tableCellTagName).Append(" ");
            this.WriteAttributes(stringBuilder, cell);
            stringBuilder.Append(">");

            this.WriteContent(stringBuilder, cell);

            stringBuilder.Append("</").Append(tableCellTagName).Append(">");
        }

        private void WriteAttributes(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            this.WriteAttribute(stringBuilder, "rowSpan", cell.RowSpan.ToString(), "1");
            this.WriteAttribute(stringBuilder, "colSpan", cell.ColumnSpan.ToString(), "1");
            this.WriteAttribute(stringBuilder, "class", string.Join(" ", cell.CssClasses));
            this.WriteAttribute(
                stringBuilder,
                "style",
                string.Join(
                    " ",
                    cell.Styles
                        .Select(x => $"{x.Key}: {x.Value};")
                )
            );

            foreach ((string name, string value) in cell.Attributes)
            {
                this.WriteAttribute(stringBuilder, name, value);
            }
        }

        private void WriteAttribute(StringBuilder stringBuilder, string name, string value, string defaultValue = "")
        {
            if (value.Equals(defaultValue, StringComparison.Ordinal))
            {
                return;
            }

            stringBuilder.Append(name).Append(@"=""").Append(HttpUtility.HtmlAttributeEncode(value)).Append(@""" ");
        }

        private void WriteContent(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            stringBuilder.Append(cell.Html);
        }
    }
}
