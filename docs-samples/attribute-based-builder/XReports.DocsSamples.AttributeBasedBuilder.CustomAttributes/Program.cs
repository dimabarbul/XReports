using XReports.Converter;
using XReports.Html;
using XReports.Html.Writers;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.AttributeHandlers;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

User[] users =
{
    new User() { Name = "John Doe", Email = "johndoe@example.com" },
    new User() { Name = "Jane Doe", Email = "janedoe@example.com" },
};

IAttributeBasedBuilder builder = new AttributeBasedBuilder(
    new[]
    {
        new MyAttributeHandler(),
    });
IReportSchema<User> schema = builder.BuildSchema<User>();

IReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>(
    new[]
    {
        new MyPropertyHandler(),
    });

IReportTable<ReportCell> reportTable = schema.BuildReportTable(users);
IReportTable<HtmlReportCell> htmlReport = converter.Convert(reportTable);

IHtmlStringWriter writer = new HtmlStringWriter(new HtmlStringCellWriter());
string html = writer.WriteToString(htmlReport);

Console.WriteLine(html);

internal class User
{
    [ReportColumn(1, "Name")]
    [My]
    public string Name { get; set; }

    [ReportColumn(2, "Email")]
    public string Email { get; set; }
}

// Property that will be assigned to cells when MyAttribute is used.
// Assigning will be done in attribute handler class.
internal class MyProperty : ReportCellProperty
{
}

// Handling cells with MyProperty during conversion to Html report.
// As an example we assign Html class "my-class" to distinguish cells with
// MyProperty.
internal class MyPropertyHandler : PropertyHandler<MyProperty, HtmlReportCell>
{
    protected override void HandleProperty(MyProperty property, HtmlReportCell cell)
    {
        cell.CssClasses.Add("my-class");
    }
}

// BasePropertyAttribute has IsHeader property allowing assigning property
// to header cells.
// Also it specifies attribute usage.
internal class MyAttribute : BasePropertyAttribute
{
}

// You can implement IAttributeHandler or inherit AttributeHandler class that
// is designed to handle one type of attribute.
internal class MyAttributeHandler : AttributeHandler<MyAttribute>
{
    // Will be called after column is added.
    protected override void HandleAttribute<TSourceItem>(
        IReportSchemaBuilder<TSourceItem> schemaBuilder,
        IReportColumnBuilder<TSourceItem> columnBuilder,
        MyAttribute attribute)
    {
        if (attribute.IsHeader)
        {
            columnBuilder.AddHeaderProperties(new MyProperty());
        }
        else
        {
            columnBuilder.AddProperties(new MyProperty());
        }
    }
}
