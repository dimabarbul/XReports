# Html Writers

The library provides 2 writers for Html reports: HtmlStringWriter and HtmlStreamWriter. HtmlStringWriter class provides ability to write Html report to string using System.Text.StringBuilder. HtmlStreamWriter allows writing report to stream. Both classes are thread-safe.

Internally both writers have similar structure, so examples below are valid for them. Both of them use cell writers (IHtmlStringCellWriter and IHtmlStreamCellWriter) that allows changing the way cells are written easily.

## .NET Core Integration

You need to call extension method AddHtmlStringWriter/AddHtmlStreamWriter. There are 3 forms of the methods.

```c#
// Registers HtmlStringWriter as implementation of IHtmlStringWriter and
// HtmlStringCellWriter as implementation of IHtmlStringCellWriter.
services.AddHtmlStringWriter();

class MyWriter : HtmlStringWriter {}
// Registers MyWriter class as implementation of IHtmlStringWriter and
// HtmlStringCellWriter as implementation of IHtmlStringCellWriter.
services.AddHtmlStringWriter<MyWriter>();

class MyWriter : HtmlStringWriter {}
class MyCellWriter : HtmlStringCellWriter {}
// Registers MyWriter as implementation of IHtmlStringWriter and MyCellWriter
// as implementation of IHtmlStringCellWriter.
services.AddHtmlStringWriter<MyWriter, MyCellWriter>();
// Or if you need just your custom cell writer.
services.AddHtmlStringWriter<HtmlStringWriter, MyCellWriter>();

// Similarly you can register stream writer.
services.AddHtmlStreamWriter();
services.AddHtmlStreamWriter<MyStreamWriter>();
services.AddHtmlStreamWriter<MyStreamWriter, MyStreamCellWriter>();
services.AddHtmlStreamWriter<HtmlStreamWriter, MyStreamCellWriter>();

```

All of the forms accept optional service lifetime, by default it's singleton.

## Extending

### Extending writer

[Working example](../../docs-samples/html-writers/XReports.DocsSamples.HtmlWriters.ExtendingWriter/Program.cs)

HtmlStringWriter class provides a number of virtual methods that can be overridden to customize result.

For example, by default report has no title. To implement report title functionality we can create TitleProperty (refer to [table properties](../xreports.core/properties.md#table-properties) for more information) that will hold report title, and extend writer to take TitleProperty into consideration.

```c#
class TitleProperty : ReportTableProperty
{
    public TitleProperty(string title)
    {
        this.Title = title;
    }

    public string Title { get; }
}

class MyHtmlStringWriter : HtmlStringWriter
{
    public MyHtmlStringWriter(IHtmlStringCellWriter htmlStringCellWriter)
        : base(htmlStringCellWriter)
    {
    }

    protected override void BeginTable(StringBuilder stringBuilder, IReportTable<HtmlReportCell> reportTable)
    {
        base.BeginTable(stringBuilder, reportTable);

        TitleProperty titleProperty = reportTable.GetProperty<TitleProperty>();
        if (titleProperty != null)
        {
            stringBuilder
                .Append("<caption>")
                .Append(titleProperty.Title)
                .Append("</caption>");
        }
    }
}

class MyHtmlStreamWriter : HtmlStreamWriter
{
    public MyHtmlStreamWriter(IHtmlStreamCellWriter htmlStringCellWriter)
        : base(htmlStringCellWriter)
    {
    }

    protected override async Task BeginTableAsync(StreamWriter streamWriter, IReportTable<HtmlReportCell> reportTable)
    {
        await base.BeginTableAsync(streamWriter, reportTable);

        TitleProperty titleProperty = reportTable.GetProperty<TitleProperty>();
        if (titleProperty != null)
        {
            await streamWriter.WriteAsync("<caption>");
            await streamWriter.WriteAsync(titleProperty.Title);
            await streamWriter.WriteAsync("</caption>");
        }
    }
}

builder.AddTableProperties(new TitleProperty("Users"));

// If you use DI, register MyHtmlWriter as implementation of IHtmlStringWriter.
services.AddHtmlStringWriter<MyHtmlWriter>();
// If you don't use DI, just create instance.
IHtmlStringWriter writer = new MyHtmlWriter(new HtmlStringCellWriter());
```

### Extending HtmlStringCellWriter

[Working example](../../docs-samples/html-writers/XReports.DocsSamples.HtmlWriters.ExtendingCellWriter/Program.cs)

Another extension point is IHtmlStringCellWriter. Writer uses cell writer to write cells one by one. Provided out-of-the-box implementation of IHtmlStringCellWriter - HtmlStringCellWriter - writes `td` (for body cells) or `th` (for header cells) tag with all attributes and content right inside it. If you need to, for example, wrap cell content in `div` and apply all attributes to it, extend HtmlStringCellWriter and override corresponding methods.

```c#
class MyHtmlStringCellWriter : HtmlStringCellWriter
{
    protected override void BeginWrappingElement(StringBuilder stringBuilder, HtmlReportCell cell, string tableCellTagName)
    {
        stringBuilder
            .Append('<')
            .Append(tableCellTagName)
            .Append("><div");
        this.WriteAttributes(stringBuilder, cell);
        stringBuilder.Append('>');
    }

    protected override void EndWrappingElement(StringBuilder stringBuilder, string tableCellTagName)
    {
        stringBuilder
            .Append("</div></")
            .Append(tableCellTagName)
            .Append('>');
    }
}

class MyHtmlStreamCellWriter : HtmlStreamCellWriter
{
    protected override async Task BeginWrappingElementAsync(StreamWriter streamWriter, HtmlReportCell cell, string tableCellTagName)
    {
        await streamWriter.WriteAsync('<');
        await streamWriter.WriteAsync(tableCellTagName);
        await streamWriter.WriteAsync("><div");
        await this.WriteAttributesAsync(streamWriter, cell);
        await streamWriter.WriteAsync('>');
    }

    protected override async Task EndWrappingElementAsync(StreamWriter streamWriter, string tableCellTagName)
    {
        await streamWriter.WriteAsync("</div></");
        await streamWriter.WriteAsync(tableCellTagName);
        await streamWriter.WriteAsync('>');
    }
}

// If you use DI, register MyHtmlStringCellWriter as implementation of IHtmlStringCellWriter.
services.AddHtmlStringWriter<HtmlStringWriter, MyHtmlStringCellWriter>();
// If you don't use DI, just create instance.
IHtmlStringWriter writer = new HtmlStringWriter(new MyHtmlStringCellWriter());
```
