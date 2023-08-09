using XReports.DocsSamples.Common;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

ReportSchemaBuilder<UserInfo> builder = new ReportSchemaBuilder<UserInfo>();

builder.AddColumn("Username", u => u.Username)
    // each cell in this column will have UpperCaseProperty
    .AddProperties(new UpperCaseProperty());
builder.AddColumn("Password", u => u.Password)
    // each cell in this column will have ProtectedProperty
    .AddProperties(new ProtectedProperty('*'));
builder.AddTableProperties(new TitleProperty("User report"));

IReportSchema<UserInfo> schema = builder.BuildVerticalSchema();

UserInfo[] users = new UserInfo[]
{
    new UserInfo() { Username = "guest", Password = "guest" },
    new UserInfo() { Username = "admin", Password = "p@$sw0rd" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(users);

MyConsoleWriter writer = new MyConsoleWriter();
writer.Write(reportTable);

internal class UpperCaseProperty : IReportCellProperty
{
}

internal record ProtectedProperty(char Symbol) : IReportCellProperty;

// ConsoleWriter class writes report table to console, but is has no
// knowledge about our properties, so we'll extend it.
internal class MyConsoleWriter : ConsoleWriter
{
    public override void Write(IReportTable<ReportCell> reportTable)
    {
        if (reportTable.TryGetProperty(out TitleProperty titleProperty))
        {
            Console.WriteLine($"*** {titleProperty.Title} ***");
        }

        base.Write(reportTable);
    }

    protected override void WriteCell(ReportCell reportCell, int cellWidth)
    {
        string text = reportCell.GetValue<string>();

        // HasProperty method returns true if cell has assigned property of provided type.
        if (reportCell.HasProperty<UpperCaseProperty>())
        {
            text = text.ToUpperInvariant();
        }

        // For ProtectedProperty it's not enough to know that it's assigned as we need to know symbol to mask the value.
        if (reportCell.TryGetProperty(out ProtectedProperty protectedProperty))
        {
            text = new string(protectedProperty.Symbol, text.Length);
        }

        Console.Write($"{{0,{cellWidth}}}", text);
    }
}

internal class UserInfo
{
    public string Username { get; set; }
    public string Password { get; set; }
}

internal class TitleProperty : IReportTableProperty
{
    public TitleProperty(string title)
    {
        this.Title = title;
    }

    public string Title { get; }
}
