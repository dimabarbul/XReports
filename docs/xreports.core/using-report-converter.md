# Using Report Converter

Working with generic report cells is totally fine, but it makes you keep logic related to properties processing in writer class. This violates single responsibility principle, which makes supporting writer classes harder. Also at some point you might have more than one writer and you'll need to share the logic somehow.

This is the main benefit of using converter - unless cell model is changed, you don't need to change your writer class(es) if you want to add more properties.

[Working example](samples/using-report-converter/XReports.DocsSamples.UsingReportConverter/Program.cs)

## Custom Model

The first step is to create custom report cell model. It should be inherited from ReportCell and may contain any extra properties you like.

This step is optional, but highly recommended. You can convert report from ReportCell to ReportCell, but if later you decide that you need more type-specific properties in your cells, you'll have hard time to update your code.
 
Let's imagine that we want to work with Html report. In this case our model might look like following:

```c#
class HtmlCell : ReportCell
{
    // Contains styles to be applied to the cell.
    public List<string> Styles { get; private set; } = new List<string>();

    // Resets cell to its initial state.
    public override void Clear()
    {
        base.Clear();

        this.Styles.Clear();
    }

    // Makes the cell clone. Base method makes a shallow copy, so all complex properties
    // should be handled here.
    public override ReportCell Clone()
    {
        HtmlCell reportCell = (HtmlCell)base.Clone();

        reportCell.Styles = new List<string>(this.Styles);

        return reportCell;
    }
}
```

## Writer

Styles class property will be used by writer class, but pay attention that writer class will no longer have to process cell properties, but only class properties that are already updated according to cell properties assigned to the cell.

```c#
class HtmlWriter
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
```

Now we can create report and display it in HTML format.

## Converter

Let's create reports displaying users information: username and email.

```c#
class UserInfo
{
    public string Username { get; set; }
    public string Email { get; set; }
}

ReportSchemaBuilder<UserInfo> builder = new ReportSchemaBuilder<UserInfo>();

builder.AddColumn("Username", (UserInfo u) => u.Username);
builder.AddColumn("Email", (UserInfo u) => u.Email);

builder.AddComplexHeader(0, "User Info", 0, 1);

IReportSchema<UserInfo> schema = builder.BuildVerticalSchema();

UserInfo[] users = new UserInfo[]
{
    new UserInfo() { Username = "guest", Email = "guest@example.com" },
    new UserInfo() { Username = "admin", Email = "admin@gmail.com" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(users);

IReportConverter<HtmlCell> converter = new ReportConverter<HtmlCell>();
IReportTable<HtmlCell> htmlReportTable = converter.Convert(reportTable);

HtmlWriter writer = new HtmlWriter();
writer.Write(htmlReportTable);
```

We've created converter instance to make report cells of type HtmlCell. For each ReportCell it produces instance of our class (HtmlCell) and copies span and value information.

So far so good, report is printed and complex header spans 2 columns.

```html
<table><thead>
<tr><th colSpan="2">User Info</th></tr>
<tr><th>Username</th><th>Email</th></tr>
</thead><tbody>
<tr><td>guest</td><td>guest@example.com</td></tr>
<tr><td>admin</td><td>admin@gmail.com</td></tr>
</tbody></table>
```

## Property and Property Handler

Now let's highlight username column in bold so report viewers will be able to easily find it. To do this we'll need cell property to mark cells that need to have such behavior.

```c#
class BoldProperty : ReportCellProperty
{
}
```

As you can see this class does not have any code to update cell. It is used only to mark the cells. To update cells that have this property assigned we need to create handler class.

```c#
// PropertyHandler class can be used if you want to handle only properties of one type.
// The handler will be called for inherited properties as well.
// Otherwise you may implement IPropertyHandler<TReportCell> interface and process all properties.
class BoldPropertyHandler : PropertyHandler<BoldProperty, HtmlCell>
{
    protected override void HandleProperty(BoldProperty property, HtmlCell cell)
    {
        cell.Styles.Add("font-weight: bold");
    }
}
```

In order to use this handler class during conversion it should be passed to converter constructor.

```c#
…
IReportConverter<HtmlCell> converter = new ReportConverter<HtmlCell>(new[]
{
    new BoldPropertyHandler(),
});
…
```

If you have many handlers, it might be awkward to add them one by one. You may use TypesCollection type to load types:

```c#
IReportConverter<HtmlCell> converter = new ReportConverter<HtmlCell>(
    new TypesCollection<IPropertyHandler<HtmlCell>>()
        .AddFromAssembly(typeof(BoldProperty).Assembly)
        .Remove<UnnecessaryHandler>()
        .Select(t => (IPropertyHandler<HtmlCell>)Activator.CreateInstance(t)));
```

Now we can assign the property to username column.

```c#
…
builder.AddColumn("Username", (UserInfo u) => u.Username)
    .AddProperties(new BoldProperty());
…
```

And we get our report.

```html
<table><thead>
<tr><th colSpan="2">User Info</th></tr>
<tr><th>Username</th><th>Email</th></tr>
</thead><tbody>
<tr><td style="font-weight: bold;">guest</td><td>guest@example.com</td></tr>
<tr><td style="font-weight: bold;">admin</td><td>admin@gmail.com</td></tr>
</tbody></table>
```

As you can see, we adjusted report without modifying writer class. If in future we want to add more properties that can be applied using HTML style, we'll be able to do so by creating properties and property handlers classes. Surely, if HtmlCell class changes, it should be reflected in writer class as well, but such changes are expected to happen much less regularly than adding properties.

## Property Handler Priority

Every property handler has priority. The lower priority is, the earlier handler is called. If it's important to run one handlers before others, you can use Priority getter to control execution order.

```c#
public class ShouldBeCalledFirstPropertyHandler : PropertyHandler<SomeProperty, ReportCell>
{
    public override int Priority => -1;

    protected override void HandleProperty(SomeProperty property, ReportCell cell)
    {
        …
    }
}
```

Default priority for classes inherited from PropertyHandler is 0.

## Property Inheritance

[Working example](samples/using-report-converter/XReports.DocsSamples.UsingReportConverter.PropertyInheritance/Program.cs)

Sometimes you might want to extend or adjust behavior of some property. To do this you can use properties inheritance. PropertyHandler class handles inherited properties, so all handlers inherited from it will process whole properties hierarchies. This allows adding more specific properties along with handlers that contain some custom logic for inherited properties.

In this example we will use classes provided by XReports library to keep example simpler. XReport is not mandatory for property inheritance to work.

We'll use following functionality from XReports:

- HtmlReportCell - report cell type for Html reports, have Html classes, styles and custom attributes
- HtmlStringWriter, HtmlStringCellWriter - classes that write Html report to string
- MaxLengthProperty - property to limit text in cells to specified length
- MaxLengthPropertyHtmlHandler - property handler class that processes MaxLengthProperty when converting report to Html

MaxLengthPropertyHtmlHandler truncates text in cell so that resulting text and postfix text ("…" by default) is not longer than specified number of characters.

Let's imagine that we want to extend handling of MaxLengthProperty in such way that sometimes (depending on user preferences) report will have title attribute containing full text. To do so we need to create new property (inherited from MaxLengthProperty) and new handler for the new property with priority lower than priority of existing handler for MaxLengthProperty to be executed earlier.

```c#
class MyMaxLengthProperty : MaxLengthProperty
{
    public MyMaxLengthProperty(bool showFullText, int maxLength)
        : base(maxLength)
    {
        this.ShowFullText = showFullText;
    }

    public bool ShowFullText { get; }
}
```

There are 2 options how to implement handler.

The first option is to inherit from MaxLengthPropertyHtmlHandler. In this case the handler will handle both - MaxLengthProperty and MyMaxLengthProperty - properties. The drawback of this approach is that the handler should process whole MaxLengthProperty hierarchy, i.e., if you have more properties inherited from MaxLengthProperty, all of them will have to be processed in this handler. This is so because MaxLengthPropertyHtmlHandler marks property as processed in non-virtual method (inherited from PropertyHandler<>), so it cannot be overriden.

```c#
class AllMaxLengthPropertiesHtmlHandler : MaxLengthPropertyHtmlHandler
{
    public override int Priority => base.Priority - 1;

    protected override void HandleProperty(MaxLengthProperty property, HtmlReportCell cell)
    {
        switch (property)
        {
            case MyMaxLengthProperty myMaxLengthProperty:
                this.HandleProperty(myMaxLengthProperty, cell);
                break;
            default:
                base.HandleProperty(property, cell);
                break;
        }
    }

    private void HandleProperty(MyMaxLengthProperty property, HtmlReportCell cell)
    {
        // If user does not want to see full text, we fallback to default
        // inherited behavior.
        if (!property.ShowFullText)
        {
            base.HandleProperty(property, cell);

            return;
        }

        // In order not to duplicate logic of determining if text is too long,
        // we save text before handler and after. If it's different, it's been
        // truncated.
        string fullText = cell.GetValue<string>();

        base.HandleProperty(property, cell);

        if (fullText != cell.GetValue<string>())
        {
            cell.Attributes["title"] = fullText;
        }
    }
}
```

Another option is to create separate handler and use MaxLengthPropertyHtmlHandler with composition. The downside is that handler doing original job (`this.originalHandler` in the example below) is hard-coded. So if later you decide to use another handler instead of MaxLengthPropertyHtmlHandler, you'll need to change code of MyMaxLengthPropertyHtmlHandler. Alternative way would be to accept the handler in constructor as, for example, PropertyHandler<MaxLengthProperty, HtmlReportCell>.

```c#
class MyMaxLengthPropertyHtmlHandler : PropertyHandler<MyMaxLengthProperty, HtmlReportCell>
{
    private readonly MaxLengthPropertyHtmlHandler originalHandler = new MaxLengthPropertyHtmlHandler();

    public override int Priority => this.originalHandler.Priority - 1;

    protected override void HandleProperty(MyMaxLengthProperty property, HtmlReportCell cell)
    {
        // If user does not want to see full text, we fallback to default behavior.
        if (!property.ShowFullText)
        {
            this.originalHandler.Handle(property, cell);

            return;
        }

        // In order not to duplicate logic of determining if text is too long,
        // we save text before handler and after. If it's different, it's been
        // truncated.
        string fullText = cell.GetValue<string>();

        this.originalHandler.Handle(property, cell);

        if (fullText != cell.GetValue<string>())
        {
            cell.Attributes["title"] = fullText;
        }
    }
}
```
