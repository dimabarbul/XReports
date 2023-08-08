using System.Text;
using XReports.Converter;
using XReports.Html;
using XReports.Html.PropertyHandlers;
using XReports.Html.Writers;
using XReports.ReportCellProperties;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

ReportSchemaBuilder<User> builder = new ReportSchemaBuilder<User>();
ReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>(
new[]
{
    new AlignmentPropertyHtmlHandler(),
});
User[] data =
{
    new User() { Name = "John Doe", Email = "johndoe@example.com" },
    new User() { Name = "Jane Doe", Email = "janedoe@example.com" },
};

builder.AddColumn("Name", (User user) => user.Name)
    .AddProperties(new AlignmentProperty(Alignment.Center));
builder.AddColumn("E-mail", (User user) => user.Email);
builder.AddTableProperties(new TitleProperty("Users"));
IReportSchema<User> schema = builder.BuildVerticalSchema();
IReportTable<ReportCell> reportTable = schema.BuildReportTable(data);
IReportTable<HtmlReportCell> htmlReport = converter.Convert(reportTable);

IHtmlStringWriter stringWriter = new HtmlStringWriter(new MyHtmlStringCellWriter());
string html = stringWriter.Write(htmlReport);

Console.WriteLine("Using string writer:");
Console.WriteLine(html);
Console.WriteLine();

IHtmlStreamWriter streamWriter = new HtmlStreamWriter(new MyHtmlStreamCellWriter());
await using Stream standardOutput = Console.OpenStandardOutput();
Console.WriteLine("Using stream writer:");
await streamWriter.WriteAsync(htmlReport, standardOutput);

internal class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}

internal class TitleProperty : IReportTableProperty
{
    public TitleProperty(string title)
    {
        this.Title = title;
    }

    public string Title { get; }
}

internal class MyHtmlStringCellWriter : HtmlStringCellWriter
{
    protected override void BeginWrappingHeaderElement(StringBuilder stringBuilder, HtmlReportCell cell)
    {
        stringBuilder.Append("<th><div");
        this.WriteAttributes(stringBuilder, cell);
        stringBuilder.Append('>');
    }

    protected override void EndWrappingHeaderElement(StringBuilder stringBuilder)
    {
        stringBuilder.Append("</div></th>");
    }

    protected override void BeginWrappingElement(StringBuilder stringBuilder, HtmlReportCell cell)
    {
        stringBuilder.Append("<td><div");
        this.WriteAttributes(stringBuilder, cell);
        stringBuilder.Append('>');
    }

    protected override void EndWrappingElement(StringBuilder stringBuilder)
    {
        stringBuilder.Append("</div></td>");
    }
}

internal class MyHtmlStreamCellWriter : HtmlStreamCellWriter
{
    protected override async Task BeginWrappingHeaderElementAsync(StreamWriter streamWriter, HtmlReportCell cell)
    {
        await streamWriter.WriteAsync("<th><div").ConfigureAwait(false);
        await this.WriteAttributesAsync(streamWriter, cell).ConfigureAwait(false);
        await streamWriter.WriteAsync('>').ConfigureAwait(false);
    }

    protected override Task EndWrappingHeaderElementAsync(StreamWriter streamWriter)
    {
        return streamWriter.WriteAsync("</div></th>");
    }

    protected override async Task BeginWrappingElementAsync(StreamWriter streamWriter, HtmlReportCell cell)
    {
        await streamWriter.WriteAsync("<td><div").ConfigureAwait(false);
        await this.WriteAttributesAsync(streamWriter, cell).ConfigureAwait(false);
        await streamWriter.WriteAsync('>').ConfigureAwait(false);
    }

    protected override Task EndWrappingElementAsync(StreamWriter streamWriter)
    {
        return streamWriter.WriteAsync("</div></td>");
    }
}
