# Using Report Converter

Working with generic report cells is totally fine, but it makes you keep logic related to properties processing in writer class. At some point you might have more than one writer and you'll need to share the logic somehow.

This is the main benefit of using converter - you don't need to change your writer class(es) if you want to add more properties.

## Custom Model

The first step is to create custom model. It should be inherited from BaseReportCell and may contain any extra properties you like.
 
 Let's imagine that we want to display Html report with some cells being formatted as Html and some containing only text.

```c#
class HtmlCell : BaseReportCell
{
    // True if we don't want to escape cell content.
    public bool IsHtml { get; set; }
}
```

## Writer

IsHtml property will be used by writer class, but pay attention that writer class will no longer have knowledge on how IsHtml is set.

```c#
class HtmlWriter
{
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
                string cellContent = cell.GetValue<string>();
                string htmlContent = cell.IsHtml ? cellContent : HttpUtility.HtmlEncode(cellContent);
                sb.Append($"<{htmlTag}>{htmlContent}</{htmlTag}>");
            }

            sb.Append("</tr>");

            Console.WriteLine(sb);
        }
    }
}
```

Now we can create report and display it with all cells escaped.

## Converter

Let's create reports displaying users information: username and email.

```c#
class UserInfo
{
    public string Username { get; set; }
    public string Email { get; set; }
}

VerticalReportSchemaBuilder<UserInfo> builder = new VerticalReportSchemaBuilder<UserInfo>();

builder.AddColumn("Username", (UserInfo u) => u.Username);
builder.AddColumn("Email", (UserInfo u) => u.Email);

VerticalReportSchema<UserInfo> schema = builder.BuildSchema();

UserInfo[] users = new UserInfo[]
{
    new UserInfo() { Username = "guest", Email = "guest@example.com" },
    new UserInfo() { Username = "admin", Email = "admin@gmail.com" },
    new UserInfo() { Username = "evil <script>alert(1)</script>", Email = "evil@inter.net" },
};

IReportTable<ReportCell> reportTable = schema.BuildReportTable(users);

IReportConverter<HtmlCell> converter = new ReportConverter<HtmlCell>();
IReportTable<HtmlCell> htmlReportTable = converter.Convert(reportTable);

HtmlWriter writer = new HtmlWriter();
writer.Write(htmlReportTable);
```

Notice line
```c#
IReportConverter<HtmlCell> converter = new ReportConverter<HtmlCell>();
```
We've created converter instance to make report cells of type HtmlCell. By default for each ReportCell it creates instance of our class (HtmlCell) and copies span and value information.

So far so good, report is printed and username of evil user is escaped.

```html
<table><thead>
<tr><th>Username</th><th>Email</th></tr>
</thead><tbody>
<tr><td>guest</td><td>guest@example.com</td></tr>
<tr><td>admin</td><td>admin@gmail.com</td></tr>
<tr><td>evil &lt;script&gt;alert(1)&lt;/script&gt;</td><td>evil@inter.net</td></tr>
</tbody></table>
```

## Property and Property Handler

Now let's try to display email cells as mailto links. To do this we'll need property to mark cells that need to have such behavior.

```c#
class EmailLinkProperty : ReportCellProperty
{
}
```

As you can see this class does not have any code to update cell. It is used only to mark the cells. To update cells that have this property assigned we need to create handler class.

```c#
// PropertyHandler class can be used if you want to handle only properties of one type.
// Otherwise you may implement IPropertyHandler<TReportCell> interface and process all properties.
class EmailLinkPropertyHandler : PropertyHandler<EmailLinkProperty, HtmlCell>
{
    protected override void HandleProperty(EmailLinkProperty property, HtmlCell cell)
    {
        // set IsHtml to true in order to not escape it in writer
        cell.IsHtml = true;

        // read cell content as string
        string email = cell.GetValue<string>();

        // update cell content to Html link
        cell.Value = $"<a href=\"mailto:{email}\">{email}</a>";
    }
}
```

In order to use this handler class during conversion it should be passed to converter constructor.

```c#
…
IReportConverter<HtmlCell> converter = new ReportConverter<HtmlCell>(new[]
{
    new EmailLinkPropertyHandler()
});
…
```

So now we can assign the property to email column.

```c#
…
builder.AddColumn("Email", (UserInfo u) => u.Email)
    .AddProperties(new EmailLinkProperty());
…
```

And we get our report.

```html
<table><thead>
<tr><th>Username</th><th>Email</th></tr>
</thead><tbody>
<tr><td>guest</td><td><a href="mailto:guest@example.com">guest@example.com</a></td></tr>
<tr><td>admin</td><td><a href="mailto:admin@gmail.com">admin@gmail.com</a></td></tr>
<tr><td>evil &lt;script&gt;alert(1)&lt;/script&gt;</td><td><a href="mailto:evil@inter.net">evil@inter.net</a></td></tr>
</tbody></table>
```

## Property Handler Priority

Every property handler has priority. By default it's 0 and does not guarantee order in which handler will be called (the lower priority is, the earlier handler is called). If it's important to run one handlers before others, you can use Priority getter to control execution order. Priority is set for handler class which means that all instances of the same handler will have the same priority.

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
