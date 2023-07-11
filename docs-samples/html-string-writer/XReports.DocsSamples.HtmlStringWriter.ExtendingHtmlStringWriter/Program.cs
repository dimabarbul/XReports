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

IHtmlStringWriter writer = new MyHtmlWriter(new HtmlStringCellWriter());
string html = writer.Write(htmlReport);

Console.WriteLine(html);

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

internal class MyHtmlWriter : HtmlStringWriter
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
