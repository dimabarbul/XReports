using XReports.DocsSamples.Common;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.AttributeHandlers;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

User[] users =
{
    new User() { FirstName = "John", LastName = "Doe" },
    new User() { FirstName = "Jane", LastName = "Doe" },
};

AttributeBasedBuilder builder = new AttributeBasedBuilder(new[]
{
    new TitlePropertyAttributeHandler(),
});
IReportSchema<User> schema = builder.BuildSchema<User>();

IReportTable<ReportCell> reportTable = schema.BuildReportTable(users);
new MyConsoleWriter().Write(reportTable);

internal class TitleProperty : IReportTableProperty
{
    public TitleProperty(string title)
    {
        this.Title = title;
    }

    public string Title { get; }
}

// Inherit TableAttribute class to indicate that this is table property and not
// global property.
internal class TitlePropertyAttribute : TableAttribute
{
    public TitlePropertyAttribute(string title)
    {
        this.Title = title;
    }

    public string Title { get; }
}

internal class TitlePropertyAttributeHandler : AttributeHandler<TitlePropertyAttribute>
{
    protected override void HandleAttribute<TSourceItem>(
        IReportSchemaBuilder<TSourceItem> schemaBuilder,
        IReportColumnBuilder<TSourceItem> columnBuilder,
        TitlePropertyAttribute attribute)
    {
        schemaBuilder.AddTableProperties(new TitleProperty(attribute.Title));
    }
}

[TitleProperty("User Report")]
internal class User
{
    [ReportColumn(1, "First Name")]
    public string FirstName { get; set; }

    [ReportColumn(2, "Last Name")]
    public string LastName { get; set; }
}

internal class MyConsoleWriter : ConsoleWriter
{
    public override void Write(IReportTable<ReportCell> reportTable)
    {
        if (reportTable.TryGetProperty(out TitleProperty titleProperty))
        {
            Console.WriteLine($"*** {titleProperty.Title} ***");
        }

        base.Write(reportTable);
    }
}
