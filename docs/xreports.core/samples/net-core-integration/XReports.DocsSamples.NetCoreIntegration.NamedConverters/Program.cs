using Microsoft.Extensions.DependencyInjection;
using XReports.Converter;
using XReports.DependencyInjection;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

ServiceCollection services = new ServiceCollection();
// Name "bootstrap" will be used when we need to get this particular converter.
// Register converter specifying handlers types.
services.AddReportConverter<HtmlReportCell>(
    "bootstrap",
    o =>
    {
        o.Add(typeof(BootstrapBoldPropertyHandler), typeof(BootstrapItalicPropertyHandler));
    });
// Or using marker interface.
services.AddReportConverter<HtmlReportCell, IStandardHtmlHandler>("email");
ServiceProvider serviceProvider = services.BuildServiceProvider();

ReportSchemaBuilder<int> builder = new ReportSchemaBuilder<int>();
// Each cell with odd number will have BoldProperty assigned.
builder.AddColumn("X", i => i)
    .AddDynamicProperties(i => i % 2 == 1 ? new BoldProperty() : new ItalicProperty());

IReportSchema<int> schema = builder.BuildVerticalSchema();

IReportTable<ReportCell> reportTable = schema.BuildReportTable(Enumerable.Range(1, 7));

// To get factory you need to get IReportConverterFactory<TReportCell>
// where TReportCell is your report cell class.
IReportConverterFactory<HtmlReportCell> converterFactory =
    serviceProvider.GetRequiredService<IReportConverterFactory<HtmlReportCell>>();

// Use factory to get "bootstrap" converter.
IReportConverter<HtmlReportCell> bootstrapConverter = converterFactory.Get("bootstrap");
Console.WriteLine("Bootstrap");
new HtmlWriter().Write(bootstrapConverter.Convert(reportTable));
Console.WriteLine();

// Use factory to get "email" converter.
IReportConverter<HtmlReportCell> emailConverter = converterFactory.Get("email");
Console.WriteLine("Email");
new HtmlWriter().Write(emailConverter.Convert(reportTable));

internal class BoldProperty : ReportCellProperty
{
}

internal class ItalicProperty : ReportCellProperty
{
}

internal class BootstrapBoldPropertyHandler : PropertyHandler<BoldProperty, HtmlReportCell>
{
    protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
    {
        cell.CssClasses.Add("font-weight-bold");
    }
}

internal class BootstrapItalicPropertyHandler : PropertyHandler<ItalicProperty, HtmlReportCell>
{
    protected override void HandleProperty(ItalicProperty property, HtmlReportCell cell)
    {
        cell.CssClasses.Add("font-italic");
    }
}

internal interface IStandardHtmlHandler : IPropertyHandler<HtmlReportCell>
{
}

// Useful when we don't have classes support, e.g., in email.
internal class StandardBoldPropertyHandler : PropertyHandler<BoldProperty, HtmlReportCell>, IStandardHtmlHandler
{
    protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
    {
        cell.Styles.Add("font-weight: bold");
    }
}

// Useful when we don't have classes support, e.g., in email.
internal class StandardItalicPropertyHandler : PropertyHandler<ItalicProperty, HtmlReportCell>, IStandardHtmlHandler
{
    protected override void HandleProperty(ItalicProperty property, HtmlReportCell cell)
    {
        cell.Styles.Add("font-style: italic");
    }
}
