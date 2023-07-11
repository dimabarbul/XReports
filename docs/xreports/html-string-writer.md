# HtmlStringWriter

HtmlStringWriter class provides ability to write Html report to string using System.Text.StringBuilder. It is thread-safe.

Internally HtmlStringWriter class uses instance of IHtmlStringCellWriter to write cells which allows changing the way cell is written easily.

## .NET Core Integration

You need to call extension method AddHtmlStringWriter. There are 3 forms of this method.

```c#
// Registers HtmlStringWriter as implementation of IHtmlStringWriter and
// HtmlStringCellWriter as implementation of IHtmlStringCellWriter.
services.AddHtmlStringWriter();

class MyWriter : HtmlStringWriter {}
// Registers MyWriter class as implementation of IHtmlStringWriter and
// HtmlStringCellWriter as implementation of IHtmlStringCellWriter.
services.AddHtmlStringWriter<MyExcelWriter>();

class MyWriter : HtmlStringWriter {}
class MyCellWriter : HtmlStringCellWriter {}
// Registers MyWriter as implementation of IHtmlStringWriter and MyCellWriter
// as implementation of IHtmlStringCellWriter.
services.AddHtmlStringWriter<MyWriter, MyCellWriter>();
// Or if you need just your custom cell writer.
services.AddHtmlStringWriter<HtmlStringWriter, MyCellWriter>();
```

All of the forms accept optional service lifetime, by default it's singleton.

## Extending

### Extending HtmlStringWriter

[Working example](../../docs-samples/html-string-writer/XReports.DocsSamples.HtmlStringWriter.ExtendingHtmlStringWriter/Program.cs)

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

class MyHtmlWriter : HtmlStringWriter
{
    public MyHtmlWriter(IHtmlStringCellWriter htmlStringCellWriter)
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

builder.AddTableProperties(new TitleProperty("Users"));

// If you use DI, register MyHtmlWriter as implementation of IHtmlStringWriter.
services.AddHtmlStringWriter<MyHtmlWriter>();
// If you don't use DI, just create instance.
IHtmlStringWriter writer = new MyHtmlWriter(new HtmlStringCellWriter());
```

### Extending HtmlStringCellWriter

[Working example](../../docs-samples/html-string-writer/XReports.DocsSamples.HtmlStringWriter.ExtendingHtmlStringCellWriter/Program.cs)

Another extension point is IHtmlStringCellWriter. Writer uses cell writer to write cells one by one. Provided out-of-the-box implementation of IHtmlStringCellWriter - HtmlStringCellWriter - writes `td` (for body cells) or `th` (for header cells) tag with all attributes and content right inside it. If you need to, for example, wrap cell content in `div` and apply all attributes to it, extend HtmlStringCellWriter and override corresponding methods.

```c#
class MyCellWriter : HtmlStringCellWriter
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

// If you use DI, register MyCellWriter as implementation of IHtmlStringCellWriter.
services.AddHtmlStringWriter<HtmlStringWriter, MyCellWriter>();
// If you don't use DI, just create instance.
IHtmlStringWriter writer = new HtmlStringWriter(new MyCellWriter());
```
