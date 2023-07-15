using System.Text;
using XReports.Converter;
using XReports.Html;
using XReports.Html.Writers;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

ReportSchemaBuilder<User> builder = new ReportSchemaBuilder<User>();
ReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>();
User[] data =
{
    new User() { Name = "John Doe", Email = "johndoe@example.com" },
    new User() { Name = "Jane Doe", Email = "janedoe@example.com" },
};

builder.AddColumn("Name", (User user) => user.Name);
builder.AddColumn("E-mail", (User user) => user.Email);
builder.AddTableProperties(new TitleProperty("Users"));
IReportSchema<User> schema = builder.BuildVerticalSchema();
IReportTable<ReportCell> reportTable = schema.BuildReportTable(data);
IReportTable<HtmlReportCell> htmlReport = converter.Convert(reportTable);

IHtmlStringWriter stringWriter = new MyHtmlStringWriter(new HtmlStringCellWriter());
string html = stringWriter.Write(htmlReport);

Console.WriteLine("Using string writer:");
Console.WriteLine(html);
Console.WriteLine();

IHtmlStreamWriter streamWriter = new MyHtmlStreamWriter(new HtmlStreamCellWriter());
Console.WriteLine("Using stream writer:");
await using Stream standardOutput = Console.OpenStandardOutput();
await streamWriter.WriteAsync(htmlReport, standardOutput);

internal class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}

internal class TitleProperty : ReportTableProperty
{
    public TitleProperty(string title)
    {
        this.Title = title;
    }

    public string Title { get; }
}

internal class MyHtmlStringWriter : HtmlStringWriter
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

internal class MyHtmlStreamWriter : HtmlStreamWriter
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
