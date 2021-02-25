# Properties

Apart from value and span information each report cell has properties. Properties describe some cell characteristics. It can be anything: font size, font weight, alignment etc.

Properties can be used by writer when report is being written in some form.

## Create Property

Let's create some properties to see how they can be used. Imagine that we want report where some cells are emphasised by converting their text to upper case. Also we'd like to have some cells protected by hiding their text using some mask symbol.

From programming stand point property is a class inherited from XReports.Models.ReportCellProperty.

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

// Writer class. Its purpose is to "write" report. This is the place where properties will come into play.
// Writers do not have any interface. It's up to you to decide what method(s) it will contain.
class ConsoleWriter
{
    // Method to print report to console.
    public void Write(IReportTable<ReportCell> reportTable)
    {
        this.WriteRows(reportTable.HeaderRows);
        Console.WriteLine(new string('-', 2 * 23 - 3));
        this.WriteRows(reportTable.Rows);
    }

    // Autxiliary method to print rows (header or body).
    private void WriteRows(IEnumerable<IEnumerable<ReportCell>> rows)
    {
        Console.WriteLine(
            string.Join(
                "\n",
                rows.Select(row =>
                    string.Join(
                        " | ",
                        row
                            .Where(c => c != null)
                            .Select(c => string.Format($"{{0,-{c.ColumnSpan * 23 - 3}}}", this.GetCellText(c)))
                    )
                )
            )
        );
    }

    // Method to get text that should be displayed taking all properties into consideration.
    // All properties are handled inside this method for simplicity's sake.
    private string GetCellText(ReportCell cell)
    {
        string text = cell.GetValue<string>();

        // HasProperty method returns true if cell has assigned property of provided type.
        if (cell.HasProperty<UpperCaseProperty>())
        {
            text = text.ToUpperInvariant();
        }

        // For ProtectedProperty it's not enough to know that it's assigned as it has read-only property we need to use.
        ProtectedProperty protectedProperty = cell.GetProperty<ProtectedProperty>();
        if (protectedProperty != null)
        {
            text = new string(protectedProperty.Symbol, text.Length);
        }

        return text;
    }
}

VerticalReportSchemaBuilder<UserInfo> builder = new VerticalReportSchemaBuilder<UserInfo>();

builder.AddColumn("Username", u => u.Username)
    // each cell in this column will have provided property
    .AddProperties(new UpperCaseProperty());
builder.AddColumn("Password", u => u.Password)
    // each cell in this column will have provided property
    .AddProperties(new ProtectedProperty('*'));

VerticalReportSchema<UserInfo> schema = builder.BuildSchema();

UserInfo[] users = new UserInfo[]
{
    new UserInfo() { Username = "guest", Password = "guest" },
    new UserInfo() { Username = "admin", Password = "p@$sw0rd" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(users);

ConsoleWriter writer = new ConsoleWriter();
writer.Write(reportTable);

/*
Username             | Password            
-------------------------------------------
GUEST                | *****               
ADMIN                | ********            
*/
```

Please, note that AddProperties method does not specify which column should get the properties. This is because builder "remembers" column that was added last.

```c#
// You can use fluent syntax.
builder.AddColumn("Username", u => u.Username)
    .AddProperties(new UpperCaseProperty());

// Or add properties later.
builder.AddColumn("Username", u => u.Username);
builder.AddProperties(new UpperCaseProperty());

// If you want to switch builder to work with another column.
// Username column in example below should have been added earlier.
builder.ForColumn("Username")
    .AddProperties(new UpperCaseProperty());
```

## Header Properties

Note that in previous example header of column Username was not converted to uppercase and Password was not hidden. That's because properties were applied to body cells. If we need to apply property to header cell, we need to:
```c#
…
builder.AddColumn("Username", u => u.Username)
    // in this case header cell will have UpperCaseProperty assigned
    .AddHeaderProperties(new UpperCaseProperty());
…

/*
USERNAME             | Password            
-------------------------------------------
guest                | *****               
admin                | ********            
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
builder.AddComplexHeader(0, "User Info", 0, 1)
    // property will be assigned to all complex header cells titled "User Info"
    .AddComplexHeaderProperties("User Info", new UpperCaseProperty());
…

/*
USER INFO                                  
Username             | Password            
-------------------------------------------
guest                | *****               
admin                | ********            
*/
```

If you want to add properties to all complex header cells, you may omit its title:

```c#
builder
    // property will be assigned to all complex header cells
    .AddComplexHeaderProperties(new UpperCaseProperty());
```

## Dynamic Properties

Previously we assigned the same property to all cells in whole column. But what if property should depend on data.

Imagine that we want to highlight users with weak password. If password is less than 8 characters, we want to show the username in upper case.

```c#
…
builder.AddColumn("Username", u => u.Username)
    // using AddDynamicProperty method you can specify function returning one
    // or several properties
    .AddDynamicProperty(u =>
    {
        if (u.Password.Length < 8)
        {
            return new UpperCaseProperty();
        }

        return null;
    });
builder.AddColumn("Password", u => u.Password)
    .AddProperties(new ProtectedProperty('*'));
…

/*
Username             | Password            
-------------------------------------------
GUEST                | *****               
admin                | ********            
*/
```

## Global Properties

To assign property to all columns/rows you may use AddGlobalProperties method.

```c#
// All columns/rows (including added later) will have this property(-ies) assigned.
builder.AddGlobalProperties(new UpperCaseProperty());
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

builder.AddTableProperties(new TitleProperty("The report title"));
```

Unlike cell properties, there are no handlers for table properties, the only way to process table properties is code in writer class. During report conversion all table properties are simply copied.
