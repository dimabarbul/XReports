# Cell Processors

Cell processors allow you to perform any actions on cells [during conversion](./using-report-converter.md). In most cases it is enough to use properties and dynamic properties, but in some rare cases you may want to use cell processors.

Let's imagine that we want to highlight even and odd rows differently. Let's align them differently so that they are easy to distinguish visually.

[Working example](../../docs-samples/cell-processors/XReports.DocsSamples.CellProcessors/Program.cs)

## Preparation

Before we create our processor, let's prepare other stuff.

### Custom Cell Model

Cells will need to know alignment assigned to them. To do so we'll create our custom cell model and add alignment there:

```c#
// Custom cell model contains additional information we need.
class ConsoleCell : ReportCell
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
```

### Writer

In order to handle cell color we'll need our writer class. In this example we'll use ConsoleWriter class as a base:

```c#
// Custom writer class adds support for our custom model.
class CustomConsoleWriter : ConsoleWriter
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
```

## Cell Processor

Cell processor should implement IReportCellProcessor interface. The processor will be called sequentially for each row for each cell.

```c#
// Processor knows only about cell to process and current data source item.
// We will treat changing data source item as move to next row.
// This logic will work only for vertical report as for horizontal report
// data source item corresponds to column, not row.
class StripedProcessor : IReportCellProcessor<UserInfo>
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

// Cell property to mark cells alignment.
class AlignmentProperty : ReportCellProperty
{
    public AlignmentProperty(bool isLeftAligned)
    {
        this.IsLeftAligned = isLeftAligned;
    }

    public bool IsLeftAligned { get; }
}

// Handler that updates model based on assigned AlignmentProperty.
class AlignmentPropertyHandler : PropertyHandler<AlignmentProperty, ConsoleCell>
{
    protected override void HandleProperty(AlignmentProperty property, ConsoleCell cell)
    {
        cell.IsLeftAligned = property.IsLeftAligned;
    }
}
```

Notice one thing: processor works with ReportCell class, not our custom. This is done on purpose. Processors are executed on cells before conversion, at this point we have only cells of base model. The processors single responsibility is to mark cells with properties. How particular property is applied to particular cell model is responsibility of property handlers which will be called after processors on cells of custom model.

Processors have access to all properties assigned using AddProperties and AddGlobalProperties (even those added after call to AddProcessors), and to properties added using AddDynamicProperties before call to AddProcessors.

```c#
// In this case processor will see CustomProperty add by AddDynamicProperties.
builder.AddColumn("Username", (UserInfo u) => u.Username)
    .AddDynamicProperties(_ => new CustomProperty())
    .AddProcessors(processor);

// In this case it won't see CustomProperty.
builder.AddColumn("Username", (UserInfo u) => u.Username)
    .AddProcessors(processor)
    .AddDynamicProperties(_ => new CustomProperty());
```

The reason behind this is that AddDynamicProperties, in fact, adds processor of type DynamicPropertiesCellProcessor, and processors are executed in the order they were added.

## Generate Report

```c#
// Data source class.
class UserInfo
{
    public string Username { get; set; }
    public string Email { get; set; }
}

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

/*
|             Username |                Email |
|----------------------|----------------------|
|                guest |    guest@example.com |
| admin                | admin@gmail.com      |
|                user1 |    user1@example.com |
| user2                | user2@example.com    |
*/
```

##  Header Cell Processor

Just like for body cells, you can add processor for header cells. You can define processors only for regular header cells, not complex header ones.
