using System.Linq;
using System.Text;
using System.Web;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.Writers
{
    public class HtmlStringCellWriter : IHtmlStringCellWriter
    {
        public void WriteHeaderCell(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            this.WriteCell(stringBuilder, cell, "th");
        }

        public void WriteBodyCell(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            this.WriteCell(stringBuilder, cell, "td");
        }

        protected virtual void WriteCell(StringBuilder stringBuilder, HtmlReportCell cell, string tableCellTagName)
        {
            this.BeginWrappingElement(stringBuilder, cell, tableCellTagName);

            this.WriteContent(stringBuilder, cell);

            this.EndWrappingElement(stringBuilder, tableCellTagName);
        }

        protected virtual void BeginWrappingElement(StringBuilder stringBuilder, HtmlReportCell cell, string tableCellTagName)
        {
            stringBuilder.Append('<').Append(tableCellTagName).Append(' ');
            this.WriteAttributes(stringBuilder, cell);
            stringBuilder.Append('>');
        }

        protected virtual void EndWrappingElement(StringBuilder stringBuilder, string tableCellTagName)
        {
            stringBuilder.Append("</").Append(tableCellTagName).Append('>');
        }

        protected void WriteAttributes(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            if (cell.RowSpan > 1)
            {
                this.WriteAttribute(stringBuilder, "rowSpan", cell.RowSpan.ToString());
            }

            if (cell.ColumnSpan > 1)
            {
                this.WriteAttribute(stringBuilder, "colSpan", cell.ColumnSpan.ToString());
            }

            if (cell.CssClasses.Count > 0)
            {
                this.WriteAttribute(stringBuilder, "class", string.Join(' ', cell.CssClasses));
            }

            if (cell.Styles.Count > 0)
            {
                this.WriteAttribute(
                    stringBuilder,
                    "style",
                    string.Join(
                        ' ',
                        cell.Styles
                            .Select(x => $"{x.Key}: {x.Value};")));
            }

            foreach ((string name, string value) in cell.Attributes)
            {
                this.WriteAttribute(stringBuilder, name, value);
            }
        }

        protected void WriteContent(StringBuilder stringBuilder, HtmlReportCell cell)
        {
            string value = cell.GetValue<string>();
            if (!cell.IsHtml)
            {
                value = HttpUtility.HtmlEncode(value);
            }

            stringBuilder.Append(value);
        }

        protected void WriteAttribute(StringBuilder stringBuilder, string name, string value)
        {
            stringBuilder.Append(name).Append(@"=""").Append(HttpUtility.HtmlAttributeEncode(value)).Append(@""" ");
        }
    }
}
