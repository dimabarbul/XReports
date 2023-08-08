using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using XReports.Converter;
using XReports.DependencyInjection;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

ServiceCollection services = new ServiceCollection();
// Use all handlers from executing assembly.
services.AddReportConverter<HtmlReportCell>(o =>
{
    o.AddFromAssembly(Assembly.GetExecutingAssembly());
});
ServiceProvider serviceProvider = services.BuildServiceProvider();

ReportSchemaBuilder<int> builder = new ReportSchemaBuilder<int>();
// Each cell with odd number will have BoldProperty assigned.
builder.AddColumn("X", i => i)
    .AddDynamicProperties(i => i % 2 == 1 ? new BoldProperty() : new ItalicProperty());

IReportSchema<int> schema = builder.BuildVerticalSchema();

IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Range(1, 7));

// As no handler was registered, we will get Html table without any styles.
IReportConverter<HtmlReportCell> converter = serviceProvider.GetRequiredService<IReportConverter<HtmlReportCell>>();

IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(reportTable);
HtmlWriter writer = new HtmlWriter();
writer.Write(htmlReportTable);

internal class BoldProperty : IReportCellProperty
{
}

internal class BoldPropertyHandler : PropertyHandler<BoldProperty, HtmlReportCell>
{
    protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
    {
        cell.Styles.Add("font-weight: bold");
    }
}

internal class ItalicProperty : IReportCellProperty
{
}

internal class ItalicPropertyHandler : PropertyHandler<ItalicProperty, HtmlReportCell>
{
    protected override void HandleProperty(ItalicProperty property, HtmlReportCell cell)
    {
        cell.Styles.Add("font-style: italic");
    }
}
