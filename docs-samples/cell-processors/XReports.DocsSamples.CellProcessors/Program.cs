using XReports.Converter;
using XReports.DocsSamples.Common;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

UserInfo[] users = new UserInfo[]
{
    new UserInfo() { Username = "guest", Email = "guest@example.com" },
    new UserInfo() { Username = "admin", Email = "admin@gmail.com" },
    new UserInfo() { Username = "user1", Email = "user1@example.com" },
    new UserInfo() { Username = "user2", Email = "user2@example.com" },
};

ReportSchemaBuilder<UserInfo> builder = new ReportSchemaBuilder<UserInfo>();

// The processor will be called for each row for each cell.
// It will assign corresponding property to cells in even or odd rows.
StripedProcessor processor = new StripedProcessor();

// Assign the processor to all columns we want to be processed.
builder.AddColumn("Username", (UserInfo u) => u.Username)
    .AddProcessors(processor);
builder.AddColumn("Email", (UserInfo u) => u.Email)
    .AddProcessors(processor);

IReportSchema<UserInfo> schema = builder.BuildVerticalSchema();

IReportTable<ReportCell> reportTable = schema.BuildReportTable(users);

// Processor will just assign properties, handler will update cell model accordingly.
IReportConverter<ConsoleCell> converter = new ReportConverter<ConsoleCell>(new[]
{
    new AlignmentPropertyHandler(),
});
IReportTable<ConsoleCell> consoleReportTable = converter.Convert(reportTable);

CustomConsoleWriter writer = new CustomConsoleWriter();
writer.Write(consoleReportTable);

// Data source class.
internal class UserInfo
{
    public string Username { get; set; }
    public string Email { get; set; }
}

// Custom cell model contains additional information we need.
internal class ConsoleCell : ReportCell
{
    // Flag indicating whether content should be left-aligned (if true)
    // or right-aligned (if false), or default behaviour of writer class (if null).
    public bool? IsLeftAligned { get; set; }

    // Resets cell to its initial state.
    public override void Clear()
    {
        base.Clear();

        this.IsLeftAligned = null;
    }
}

// Custom writer class adds support for our custom model.
internal class CustomConsoleWriter : ConsoleWriter
{
    protected override void WriteCell(ReportCell reportCell, int cellWidth)
    {
        // Apply custom logic only to our cell model.
        if (reportCell is not ConsoleCell { IsLeftAligned: not null } consoleCell)
        {
            base.WriteCell(reportCell, cellWidth);

            return;
        }

        string cellContent = consoleCell.GetValue<string>();
        if (consoleCell.IsLeftAligned.Value)
        {
            Console.Write($"{{0,-{cellWidth}}}", cellContent);
        }
        else
        {
            Console.Write($"{{0,{cellWidth}}}", cellContent);
        }
    }
}

// Cell property to mark cells alignment.
internal class AlignmentProperty : ReportCellProperty
{
    public AlignmentProperty(bool isLeftAligned)
    {
        this.IsLeftAligned = isLeftAligned;
    }

    public bool IsLeftAligned { get; }
}

// Handler that updates model based on assigned AlignmentProperty.
internal class AlignmentPropertyHandler : PropertyHandler<AlignmentProperty, ConsoleCell>
{
    protected override void HandleProperty(AlignmentProperty property, ConsoleCell cell)
    {
        cell.IsLeftAligned = property.IsLeftAligned;
    }
}

// Processor knows only about cell to process and current data source item.
// We will treat changing data source item as move to next row.
// This logic will work only for vertical report as for horizontal report
// data source item corresponds to column, not row.
internal class StripedProcessor : IReportCellProcessor<UserInfo>
{
    // Properties to assign. It makes sense to reuse same properties instead
    // creating new ones for each cell.
    private static readonly AlignmentProperty OddProperty = new AlignmentProperty(false);
    private static readonly AlignmentProperty EvenProperty = new AlignmentProperty(true);

    // At the beginning we are at row 0, it's even.
    private AlignmentProperty currentProperty = EvenProperty;
    private UserInfo lastUserInfo;

    public void Process(ReportCell cell, UserInfo item)
    {
        // If data source item has changed, we started a new row.
        if (item != this.lastUserInfo)
        {
            this.currentProperty = this.currentProperty == OddProperty ?
                EvenProperty :
                OddProperty;
            this.lastUserInfo = item;
        }

        cell.AddProperty(this.currentProperty);
    }
}
