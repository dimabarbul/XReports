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

IHtmlStringWriter writer = new HtmlStringWriter(new MyCellWriter());
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

internal class MyCellWriter : HtmlStringCellWriter
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
