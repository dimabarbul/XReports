# .NET Core Integration

XReports.Core library provides extension method to register report converter.

To see it in action let's create custom cell class, custom property and writer.

```c#
class HtmlReportCell : BaseReportCell
{
    public List<string> CssClasses { get; set; } = new List<string>();
    public List<string> Styles { get; set; } = new List<string>();

    public override void Clear()
    {
        base.Clear();

        this.CssClasses.Clear();
        this.Styles.Clear();
    }
}

class BoldProperty : ReportCellProperty
{
}

// Useful if we have Bootstrap 4 installed.
class BoldPropertyBootstrapHandler : PropertyHandler<BoldProperty, HtmlReportCell>
{
    protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
    {
        cell.CssClasses.Add("font-weight-bold");
    }
}

// Might be used to register handlers implementing it.
interface IHtmlReportCellHandler : IPropertyHandler<HtmlReportCell>
{
}

// Useful when we don't have classes support, e.g., in email.
class BoldPropertyStandardHandler : PropertyHandler<BoldProperty, HtmlReportCell>, IHtmlReportCellHandler
{
    protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
    {
        cell.Styles.Add("font-weight: bold");
    }
}

class HtmlWriter
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
                sb.Append($"<{htmlTag}")
                    .Append(cell.CssClasses.Any() ? $" class=\"{string.Join(" ", cell.CssClasses)}\"" : string.Empty)
                    .Append(cell.Styles.Any() ? $" style=\"{string.Join("; ", cell.Styles)}\"" : string.Empty)
                    .Append($">{cell.GetValue<string>()}</{htmlTag}>");
            }

            sb.Append("</tr>");

            Console.WriteLine(sb);
        }
    }
}

// Shared code. Further examples will add code below this.
VerticalReportSchemaBuilder<int> builder = new VerticalReportSchemaBuilder<int>();
// Each cell with odd number will have BoldProperty assigned.
builder.AddColumn("X", i => i)
    .AddDynamicProperty(i => i % 2 == 1 ? new BoldProperty() : null);

VerticalReportSchema<int> schema = builder.BuildSchema();

IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Range(1, 7));
```

## Default Converter

As report converter is a generic class, you need to register converter for each report cell type. In our example we have one class - HtmlReportCell, so need to register one report converter.

You can register default converter and any class depending on IReportConverter with the type will get it.

```c#
ServiceCollection services = new ServiceCollection();
// Registers converter without any handler. This is not very useful, but it works.
services.AddReportConverter<HtmlReportCell>();
ServiceProvider serviceProvider = services.BuildServiceProvider();

// As no handler was registered, we will get Html table without any styles.
IReportConverter<HtmlReportCell> converter = serviceProvider.GetRequiredService<IReportConverter<HtmlReportCell>>();

IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(reportTable);
HtmlWriter writer = new HtmlWriter();
writer.Write(htmlReportTable);
```

```html
<table><thead>
<tr><th>X</th></tr>
</thead><tbody>
<tr><td>1</td></tr>
<tr><td>2</td></tr>
<tr><td>3</td></tr>
<tr><td>4</td></tr>
<tr><td>5</td></tr>
<tr><td>6</td></tr>
<tr><td>7</td></tr>
</tbody></table>
```

While registering converter you can pass types of handlers to use.

```c#
ServiceCollection services = new ServiceCollection();
// Pass type of BoldPropertyBootstrapHandler.
services.AddReportConverter<HtmlReportCell>(o =>
{
    o.Add(typeof(BoldPropertyBootstrapHandler));
});
ServiceProvider serviceProvider = services.BuildServiceProvider();

IReportConverter<HtmlReportCell> converter = serviceProvider.GetRequiredService<IReportConverter<HtmlReportCell>>();

IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(reportTable);
HtmlWriter writer = new HtmlWriter();
writer.Write(htmlReportTable);
```

```html
<table><thead>
<tr><th>X</th></tr>
</thead><tbody>
<tr><td class="font-weight-bold">1</td></tr>
<tr><td>2</td></tr>
<tr><td class="font-weight-bold">3</td></tr>
<tr><td>4</td></tr>
<tr><td class="font-weight-bold">5</td></tr>
<tr><td>6</td></tr>
<tr><td class="font-weight-bold">7</td></tr>
</tbody></table>
```

Most likely you'll have more than one handler, so registering them like this might be awkward. You can use interface or base class and pass it while registering converter.

```c#
ServiceCollection services = new ServiceCollection();
// Second type is marker interface, so converter will use all handlers
// implementing this interface.
services.AddReportConverter<HtmlReportCell, IHtmlReportCellHandler>();
// Also you can do this using configure callback:
// services.AddReportConverter<HtmlReportCell>(o =>
// {
//     o.AddByBaseType<IHtmlReportCellHandler>();
// });
ServiceProvider serviceProvider = services.BuildServiceProvider();

IReportConverter<HtmlReportCell> converter = serviceProvider.GetRequiredService<IReportConverter<HtmlReportCell>>();

IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(reportTable);
HtmlWriter writer = new HtmlWriter();
writer.Write(htmlReportTable);
```

```html
<table><thead>
<tr><th>X</th></tr>
</thead><tbody>
<tr><td style="font-weight: bold">1</td></tr>
<tr><td>2</td></tr>
<tr><td style="font-weight: bold">3</td></tr>
<tr><td>4</td></tr>
<tr><td style="font-weight: bold">5</td></tr>
<tr><td>6</td></tr>
<tr><td style="font-weight: bold">7</td></tr>
</tbody></table>
```

This 2 methods can be combined.

```c#
// This is example code, it uses classes and interfaces not provided in this doc.
services.AddReportConverter<HtmlReportCell>(o =>
{
    o.Add(typeof(MyHandler))
        .AddByBaseType<IMyHandler>();
});
```

You can add handlers from specific assembly:

```c#
// This is example code, it uses classes and interfaces not provided in this doc.
services.AddReportConverter<HtmlReportCell>(o =>
{
    o.AddFromAssembly<IMyHandler>(Assembly.GetExecutingAssembly());
});
```

## Named Converters

Sometimes you might want to have several converters, for example, one for displaying report on website, another - for sending report in email. To achieve this you can register named converters.

```c#
ServiceCollection services = new ServiceCollection();
// Name "bootstrap" will be used when we need to get this particular converter.
services.AddReportConverter<HtmlReportCell>(
    "bootstrap",
    o =>
    {
        o.Add(typeof(BoldPropertyBootstrapHandler));
    });
// You can pass interface as well.
services.AddReportConverter<HtmlReportCell, IHtmlReportCellHandler>("email");
ServiceProvider serviceProvider = services.BuildServiceProvider();

// To get factory you need to get IReportConverterFactory<TReportCell>
// where TReportCell is your report cell class.
IReportConverterFactory<HtmlReportCell> converterFactory =
    serviceProvider.GetRequiredService<IReportConverterFactory<HtmlReportCell>>();

// Use factory to get "bootstrap" converter.
IReportConverter<HtmlReportCell> bootstrapConverter = converterFactory.Get("bootstrap");
Console.WriteLine("Bootstrap");
new HtmlWriter().Write(bootstrapConverter.Convert(reportTable));
Console.WriteLine();

// Use factory to get "email" converter.
IReportConverter<HtmlReportCell> emailConverter = converterFactory.Get("email");
Console.WriteLine("Email");
new HtmlWriter().Write(emailConverter.Convert(reportTable));
```

```html
Bootstrap
<table><thead>
<tr><th>X</th></tr>
</thead><tbody>
<tr><td class="font-weight-bold">1</td></tr>
<tr><td>2</td></tr>
<tr><td class="font-weight-bold">3</td></tr>
<tr><td>4</td></tr>
<tr><td class="font-weight-bold">5</td></tr>
<tr><td>6</td></tr>
<tr><td class="font-weight-bold">7</td></tr>
</tbody></table>

Email
<table><thead>
<tr><th>X</th></tr>
</thead><tbody>
<tr><td style="font-weight: bold">1</td></tr>
<tr><td>2</td></tr>
<tr><td style="font-weight: bold">3</td></tr>
<tr><td>4</td></tr>
<tr><td style="font-weight: bold">5</td></tr>
<tr><td>6</td></tr>
<tr><td style="font-weight: bold">7</td></tr>
</tbody></table>
```

## Handlers with Dependencies

If handler has dependency, the dependency should be registered in service collection. Handler itself does not have to be registered.

## Custom Lifetime

By default converter and converter factory are registered with scoped service lifetime. It can be changed using `lifetime` argument when adding converter.

```c#
ServiceCollection services = new ServiceCollection();
services.AddReportConverter<HtmlReportCell>(lifetime: ServiceLifetime.Singleton);
```
