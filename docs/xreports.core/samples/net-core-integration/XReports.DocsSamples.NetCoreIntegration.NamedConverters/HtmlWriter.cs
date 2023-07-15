using System.Text;
using XReports.Table;

internal class HtmlWriter
{
    public void Write(IReportTable<HtmlReportCell> reportTable)
    {
        Console.WriteLine("<table><thead>");
        this.WriteRows(reportTable.HeaderRows, "th");
        Console.WriteLine("</thead><tbody>");
        this.WriteRows(reportTable.Rows, "td");
        Console.WriteLine("</tbody></table>");
    }

    private void WriteRows(IEnumerable<IEnumerable<HtmlReportCell>> rows, string htmlTag)
    {
        StringBuilder sb = new StringBuilder();
        foreach (IEnumerable<HtmlReportCell> row in rows)
        {
            sb.Clear();
            sb.Append("<tr>");

            foreach (HtmlReportCell cell in row)
            {
                sb.Append($"<{htmlTag}");

                if (cell.CssClasses.Any())
                {
                    sb.Append($" class=\"{string.Join(" ", cell.CssClasses)}\"");
                }

                if (cell.Styles.Any())
                {
                    sb.Append($" style=\"{string.Join("; ", cell.Styles)}\"");
                }

                sb.Append($">{cell.GetValue<string>()}</{htmlTag}>");
            }

            sb.Append("</tr>");

            Console.WriteLine(sb);
        }
    }
}
