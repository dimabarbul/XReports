# Properties

Apart from value and span information each report cell has properties. Properties describe some cell characteristics. It can be anything: font size, font weight, alignment etc.

The purpose of properties is to separate characteristics of report cells from how these characteristics are displayed. For instance, ProtectedProperty in examples below marks cells which content should be protected. It does not imply how this protected content should be displayed (although it has "Symbol" property that can be used during display), this decision is made in writer in the examples. Another examples of properties are alignment, font color etc. The reason of having such properties is the same - displaying content, for example, left-aligned is different for different display formats: in HTML it might be adding a class or HTML "style" attribute, in console it might be achieved by specifying alignment in string format.

Another way of handling properties is to use [property handlers](./using-report-converter.md#property-and-property-handler) with report converter.

## Cell Properties

[Working example](../../docs-samples/properties/XReports.DocsSamples.Properties.CellProperties/Program.cs)

Let's create some properties to see how they can be used. Imagine that we want report where some cells are emphasised by converting their text to upper case. Also we'd like to have some cells protected by hiding their text using some mask symbol.

From programming stand point property is a class inherited from ReportCellProperty.

```c#
class UpperCaseProperty : ReportCellProperty
{
}

class ProtectedProperty : ReportCellProperty
{
    public char Symbol { get; }

    public ProtectedProperty(char symbol)
    {
        this.Symbol = symbol;
    }
}
```

Assign properties to columns you want during building schema.

```c#
class UserInfo
{
    public string Username { get; set; }
    public string Password { get; set; }
}

ReportSchemaBuilder<UserInfo> builder = new ReportSchemaBuilder<UserInfo>();

builder.AddColumn("Username", u => u.Username)
    // each cell in this column will have UpperCaseProperty
    .AddProperties(new UpperCaseProperty());
builder.AddColumn("Password", u => u.Password)
    // each cell in this column will have ProtectedProperty
    .AddProperties(new ProtectedProperty('*'));
```

There are several ways to configure columns:

```c#
// You can use fluent syntax to configure newly added column.
builder.AddColumn("Username", u => u.Username)
    .AddProperties(new UpperCaseProperty());

// Or add properties later.
IReportSchemaCellsProviderBuilder<UserInfo> cellsBuilder = builder.AddColumn("Username", u => u.Username);
cellsBuilder.AddProperties(new UpperCaseProperty());

// To work with column added previously, you can use ForColumn method.
// Username column in example below should have been added earlier.
builder.ForColumn("Username")
    .AddProperties(new UpperCaseProperty());

// If the column has been added with ID, you can use it.
builder.ForColumn(new ColumnId("Username"))
    .AddProperties(new UpperCaseProperty());
```

Assigned properties will be available in built report table. Writer class can use them to properly display data.

```c#
// ConsoleWriter class writes report table to console, but is has no
// knowledge about our properties, so we'll extend it.
class MyConsoleWriter : ConsoleWriter
{
…
    protected override void WriteCell(ReportCell reportCell, int cellWidth)
    {
        string text = reportCell.GetValue<string>();

        // HasProperty method returns true if cell has assigned property of provided type.
        if (reportCell.HasProperty<UpperCaseProperty>())
        {
            text = text.ToUpperInvariant();
        }

        // For ProtectedProperty it's not enough to know that it's assigned as we need to know symbol to mask the value.
        ProtectedProperty protectedProperty = reportCell.GetProperty<ProtectedProperty>();
        if (protectedProperty != null)
        {
            text = new string(protectedProperty.Symbol, text.Length);
        }

        Console.Write($"{{0,{cellWidth}}}", text);
    }
}

/*
EXAMPLE OUTPUT:

|             Username |             Password |
-----------------------------------------------
|                GUEST |                ***** |
|                ADMIN |             ******** |
*/
```

## Header Properties

Note that in previous example header of column Username was not converted to uppercase and header of Password was not hidden. That's because properties were applied to body cells. If we need to apply property to header cell, we need to use method AddHeaderProperties:

```c#
…
builder.AddColumn("Username", u => u.Username)
    // in this case header cell will have UpperCaseProperty assigned
    .AddHeaderProperties(new UpperCaseProperty());
…

/*
|             USERNAME |             Password |
|----------------------|----------------------|
|                guest |                ***** |
|                admin |             ******** |
*/
```

## Complex Header Properties

Property can be assigned to complex header cell by title.

```c#
…
builder.AddColumn("Username", u => u.Username);
builder.AddColumn("Password", u => u.Password)
    .AddProperties(new ProtectedProperty('*'));

// Add complex header
builder.AddComplexHeader(0, "User Info", 0, 1);
// property will be assigned to all complex header cells titled "User Info"
builder.AddComplexHeaderProperties("User Info", new UpperCaseProperty());
…

/*
|                                   USER INFO |
|----------------------|----------------------|
|             Username |             Password |
|----------------------|----------------------|
|                guest |                ***** |
|                admin |             ******** |
*/
```

If you want to add properties to all complex header cells, you may omit its title:

```c#
builder.AddComplexHeader(0, "User Info", "Username");
builder.AddComplexHeader(0, "Security Info", "Password");
builder
    // property will be assigned to all complex header cells
    .AddComplexHeaderProperties(new UpperCaseProperty());

/*
|            USER INFO |        SECURITY INFO |
|----------------------|----------------------|
|             Username |             Password |
|----------------------|----------------------|
|                guest |                ***** |
|                admin |             ******** |
*/
```

## Dynamic Properties

Previously we assigned the same property to all cells in whole column. But what if property should depend on data.

Imagine that we want to highlight users with weak password. If password is less than 8 characters, we want to show the username in upper case.

```c#
…
UpperCaseProperty upperCaseProperty = new UpperCaseProperty();
builder.AddColumn("Username", u => u.Username)
    // Using AddDynamicProperties method you can specify function returning one
    // or several properties.
    // If returned property(-ies) is the same for all cells, it is advisable to
    // return the same object instead of creating new inside of function.
    .AddDynamicProperties(u =>
    {
        if (u.Password.Length < 8)
        {
            return upperCaseProperty;
        }

        return null;
    });
builder.AddColumn("Password", u => u.Password)
    .AddProperties(new ProtectedProperty('*'));
…

/*
|             Username |             Password |
|----------------------|----------------------|
|                GUEST |                ***** |
|                admin |             ******** |
*/
```

## Global Properties

To assign property to all columns/rows you may use AddGlobalProperties method.

```c#
builder.AddColumn("Username", u => u.Username);
builder.AddColumn("Password", u => u.Password)
    .AddProperties(new ProtectedProperty('*'));
// All columns/rows (including added later) will have this property(-ies) assigned.
builder.AddGlobalProperties(new UpperCaseProperty());

/*
|             Username |             Password |
|----------------------|----------------------|
|                GUEST |                ***** |
|                ADMIN |             ******** |
*/
```

## Table Properties

Sometimes you may need to add properties to table itself, for example, it may be report title or author information.

```c#
class TitleProperty : ReportTableProperty
{
    public TitleProperty(string title)
    {
        this.Title = title;
    }

    public string Title { get; }
}

class MyConsoleWriter : ConsoleWriter
{
    public override void Write(IReportTable<ReportCell> reportTable)
    {
        TitleProperty titleProperty = reportTable.GetProperty<TitleProperty>();
        if (titleProperty != null)
        {
            Console.WriteLine($"*** {titleProperty.Title} ***");
        }

        base.Write(reportTable);
    }
…
}

builder.AddTableProperties(new TitleProperty("User report"));

/*
*** User report ***
|             Username |             Password |
|----------------------|----------------------|
|                GUEST |                ***** |
|                ADMIN |             ******** |
*/
```

Unlike cell properties, there are no [handlers](using-report-converter.md) for table properties, the only way to process table properties is code in writer class. During report conversion all table properties are simply copied.
