# About
This project is intended to provide basic but extendable way of building reports and exporting them to different formats: HTML, Excel etc.

# Report Flow
Report Schema Builder → Report Schema → (using Data) → Generic Report Table → (converter) → Typed Report Table → (writer) → Output

There are 2 schema builders: VerticalReportSchemaBuilder and HorizontalReportSchemaBuilder. They are used to create report schema - object containing information what columns/rows report has, how data should be formatted and styled etc.

Example:
```c#
class DataItem
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// Can use dynamic if you don't know type of data.
VerticalReportSchemaBuilder<DataItem> builder = new VerticalReportSchemaBuilder<DataItem>();
builder.AddColumn("Name", x => x.Name);
builder.AddColumn("Age", x => x.Age);
VerticalReportSchema<DataItem> schema = builder.BuildSchema();

HorizontalReportSchemaBuilder<DataItem> builder2 = new HorizontalReportSchemaBuilder<DataItem>();
builder2.AddRow("Name", x => x.Name);
builder2.AddRow("Age", x => x.Age);
HorizontalReportSchema<DataItem> schema2 = builder2.BuildSchema();
```

Once you have report schema, you can build report. To do so you need data. Example:
```c#
IEnumerable<DataItem> data = new DataItem[]
{
    new DataItem { Name = "John", Age = 23 },
    new DataItem { Name = "Jane", Age = 22 },
};
IReportTable<ReportCell> reportTable = schema.BuildReportTable(data);
```

This is generic report table - it allows to iterate over header and body cells of the report. Each cell contain value (along with its type), cell span information and properties. While it's possible to work with this report table, it's not the most convenient way to get the report.

Generic report table may be converter to typed report table - report table where all cells are of one specific type - Html or Excel. They provide Html- and Excel-specific properties. For example, HtmlReportCell has such properties as CSS classes, Html attributes etc. To get typed report table you need a ReportConverter:
```c#
ReportConverter<HtmlReportCell> htmlConverter =
    new ReportConverter<HtmlReportCell>(new IPropertyHandler<HtmlReportCell>[0]);
IReportTable<HtmlReportCell> htmlReportTable = htmlConverter.Convert(reportTable);
```

Now you have report table each cell of which is HtmlReportCell. The last step is to write report somewhere. Classes that saves reports somewhere are called _writers_. They do not have common interface as they may have different names and arguments. The library comes with one writer for Excel report - EpplusWriter (it uses [Epplus 4](https://github.com/JanKallman/EPPlus)) and one writer for Html report - StringWriter. It saves report into string using StringBuilder class.

```c#
StringWriter stringWriter = new StringWriter(new StringCellWriter());
string html = await stringWriter.WriteToStringAsync(htmlReportTable);
```
