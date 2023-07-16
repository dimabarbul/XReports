using System.Text;
using System.Web;
using XReports.Converter;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

ReportSchemaBuilder<UserInfo> builder = new ReportSchemaBuilder<UserInfo>();

builder.AddColumn("Username", (UserInfo u) => u.Username)
    .AddProperties(new BoldProperty());
builder.AddColumn("Email", (UserInfo u) => u.Email);

builder.AddComplexHeader(0, "User Info", 0, 1);

IReportSchema<UserInfo> schema = builder.BuildVerticalSchema();

UserInfo[] users = new UserInfo[]
{
    new UserInfo() { Username = "guest", Email = "guest@example.com" },
    new UserInfo() { Username = "admin", Email = "admin@gmail.com" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(users);

IReportConverter<HtmlCell> converter = new ReportConverter<HtmlCell>(new[]
{
    new BoldPropertyHandler(),
});
IReportTable<HtmlCell> htmlReportTable = converter.Convert(reportTable);

HtmlWriter writer = new HtmlWriter();
writer.Write(htmlReportTable);

internal class UserInfo
{
    public string Username { get; set; }
    public string Email { get; set; }
}

internal class HtmlCell : ReportCell
{
    // Contains styles to be applied to the cell.
    public List<string> Styles { get; private set; } = new List<string>();

    // Resets cell to its initial state.
    public override void Clear()
    {
        base.Clear();

        this.Styles.Clear();
    }

    // Makes the cell deep clone. So any complex properties defined in HtmlCell,
    // like collections, should be handled here.
    public override ReportCell Clone()
    {
        HtmlCell reportCell = (HtmlCell)base.Clone();

        reportCell.Styles = new List<string>(this.Styles);

        return reportCell;
    }
}

internal class HtmlWriter
{
    // The method used to write report to console. Note that it accepts IReportTable<HtmlCell>.
    public void Write(IReportTable<HtmlCell> reportTable)
    {
        Console.WriteLine("<table><thead>");
        this.WriteRows(reportTable.HeaderRows, "th");
        Console.WriteLine("</thead><tbody>");
        this.WriteRows(reportTable.Rows, "td");
        Console.WriteLine("</tbody></table>");
    }

    private void WriteRows(IEnumerable<IEnumerable<HtmlCell>> rows, string htmlTag)
    {
        StringBuilder sb = new StringBuilder();
        foreach (IEnumerable<HtmlCell> row in rows)
        {
            sb.Clear();
            sb.Append("<tr>");

            foreach (HtmlCell cell in row)
            {
                // Spanned cells are null.
                if (cell == null)
                {
                    continue;
                }

                this.WriteCell(sb, htmlTag, cell);
            }

            sb.Append("</tr>");

            Console.WriteLine(sb);
        }
    }

    private void WriteCell(StringBuilder sb, string htmlTag, HtmlCell cell)
    {
        sb.Append($"<{htmlTag}");

        // Column and row span is inherited by cell from base class.
        this.AppendSpanInfo(sb, cell);

        // Handle our custom Styles class property.
        this.AppendStyles(sb, cell);

        string cellContent = cell.GetValue<string>();
        sb.Append($">{cellContent}</{htmlTag}>");
    }

    private void AppendSpanInfo(StringBuilder sb, HtmlCell cell)
    {
        if (cell.ColumnSpan != 1)
        {
            sb.Append($" colSpan=\"{cell.ColumnSpan}\"");
        }

        if (cell.RowSpan != 1)
        {
            sb.Append($" rowSpan=\"{cell.RowSpan}\"");
        }
    }

    private void AppendStyles(StringBuilder sb, HtmlCell cell)
    {
        if (cell.Styles.Count == 0)
        {
            return;
        }

        sb.Append(" style=\"");

        foreach (string style in cell.Styles)
        {
            sb
                .Append(HttpUtility.HtmlAttributeEncode(style))
                .Append(';');
        }

        sb.Append('"');
    }
}

internal class BoldProperty : ReportCellProperty
{
}

// PropertyHandler class can be used if you want to handle only properties of one type.
// The handler will be called for inherited properties as well.
// Otherwise you may implement IPropertyHandler<TReportCell> interface and process all properties.
internal class BoldPropertyHandler : PropertyHandler<BoldProperty, HtmlCell>
{
    protected override void HandleProperty(BoldProperty property, HtmlCell cell)
    {
        cell.Styles.Add("font-weight: bold");
    }
}
