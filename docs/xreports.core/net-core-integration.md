# .NET Core Integration

XReports.Core library provides extension methods to register report converter.

The examples below use HtmlReportCell (which provides CssClasses and Styles class properties) as a report cell class and 2 properties: BoldProperty and ItalicProperty.

## Default Converter

[Working example](samples/net-core-integration/XReports.DocsSamples.NetCoreIntegration.DefaultConverter)

You'll need to register converter for each report cell type. In our example we have one class - HtmlReportCell, so need to register one report converter.

You can register report converter, optionally along with its lifetime (singleton by default) and handlers that it should use. See [registration parameters](#registration-parameters) for more examples.

```c#
// Use all handlers from executing assembly.
services.AddReportConverter<HtmlReportCell>(o =>
{
    o.AddFromAssembly(Assembly.GetExecutingAssembly());
});
```

Example output:

```html
<table><thead>
<tr><th>X</th></tr>
</thead><tbody>
<tr><td style="font-weight: bold">1</td></tr>
<tr><td style="font-style: italic">2</td></tr>
<tr><td style="font-weight: bold">3</td></tr>
<tr><td style="font-style: italic">4</td></tr>
<tr><td style="font-weight: bold">5</td></tr>
<tr><td style="font-style: italic">6</td></tr>
<tr><td style="font-weight: bold">7</td></tr>
</tbody></table>
```

## Named Converters

Sometimes you might want to have several converters for the same report cell type, for example, one for displaying report on website, another - for sending report in email. To achieve this you can register named converters.

[Working example](samples/net-core-integration/XReports.DocsSamples.NetCoreIntegration.NamedConverters)

```c#
// Name "bootstrap" will be used when we need to get this particular converter.
// Register converter specifying handlers types.
services.AddReportConverter<HtmlReportCell>(
    "bootstrap",
    o =>
    {
        o.Add(typeof(BootstrapBoldPropertyHandler), typeof(BootstrapItalicPropertyHandler));
    });
// Or using marker interface.
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
<tr><td class="font-italic">2</td></tr>
<tr><td class="font-weight-bold">3</td></tr>
<tr><td class="font-italic">4</td></tr>
<tr><td class="font-weight-bold">5</td></tr>
<tr><td class="font-italic">6</td></tr>
<tr><td class="font-weight-bold">7</td></tr>
</tbody></table>

Email
<table><thead>
<tr><th>X</th></tr>
</thead><tbody>
<tr><td style="font-weight: bold">1</td></tr>
<tr><td style="font-style: italic">2</td></tr>
<tr><td style="font-weight: bold">3</td></tr>
<tr><td style="font-style: italic">4</td></tr>
<tr><td style="font-weight: bold">5</td></tr>
<tr><td style="font-style: italic">6</td></tr>
<tr><td style="font-weight: bold">7</td></tr>
</tbody></table>
```

## Handlers with Dependencies

If handler has dependency, the dependency should be registered in service collection. Handler itself does not have to be registered.

## Registration Parameters

During report converter registration you can configure what handlers it should use. There is a number of options you ca specify the handlers types:

```c#
services.AddReportConverter<HtmlReportCell>(o =>
{
    o
        // explicitly specify types
        .Add(typeof(Handler1), typeof(Handler2))
        // specify base type (class or interface)
        // it will add all non-abstract classes implementing IBaseHandler from all loaded assemblies
        .AddByBaseType<IBaseHandler>()
        // add all non-abstract classes implementing IPropertyHandler<HtmlReportCell>
        // from currently executing assembly
        .AddFromAssembly(Assembly.GetExecutingAssembly())
        // add all non-abstract classes implementing IBaseHandler
        // from currently executing assembly
        .AddFromAssembly<IBaseHandler>(Assembly.GetExecutingAssembly())
        // exclude not needed handler
        .Remove<NotNeededHandler>()
        // exclude multiple not needed handlers
        .Remove(typeof(NotNeededHandler1), typeof(NotNeededHandler2));
});
```

## Custom Lifetime

By default converter and converter factory are registered with singleton service lifetime. It can be changed using `lifetime` argument when adding converter.

```c#
services.AddReportConverter<HtmlReportCell>(lifetime: ServiceLifetime.Transient);
```
