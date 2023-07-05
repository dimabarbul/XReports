using Microsoft.Extensions.DependencyInjection;
using XReports.Converter;
using XReports.DependencyInjection;
using XReports.Html;
using XReports.Html.PropertyHandlers;
using XReports.Html.Writers;
using XReports.ReportCellProperties;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

ServiceCollection services = new ServiceCollection();
services
    .AddAttributeBasedBuilder(static _ => { })
    .AddReportConverter<HtmlReportCell>(
        c => c.AddFromAssembly(typeof(BoldPropertyHtmlHandler).Assembly))
    .AddHtmlStringWriter();
ServiceProvider serviceProvider = services.BuildServiceProvider();

IAttributeBasedBuilder builder = serviceProvider.GetRequiredService<IAttributeBasedBuilder>();
IReportConverter<HtmlReportCell> converter = serviceProvider.GetRequiredService<IReportConverter<HtmlReportCell>>();
IHtmlStringWriter writer = serviceProvider.GetRequiredService<IHtmlStringWriter>();
IReportTable<ReportCell> reportTable;

UserScoreModel[] data =
{
    new UserScoreModel() { Name = "John", Score = 90 },
    new UserScoreModel() { Name = "Jane", Score = 100 },
};

// Building report without providing build parameter will throw an exception
// as most likely this indicates an issue. If you want to be able to build
// report with or without parameter, make post-builder class implement both
// IReportSchemaPostBuilder<TModel,TParameter> and IReportSchemaPostBuilder<TModel>
// interfaces.
// The following line will throw exception in our example.
//builder.BuildSchema<UserScoreModel>().BuildReportTable(data);

// Both rows will have BoldProperty assigned to Score cells as both scores
// are greater than 80.
reportTable = builder.BuildSchema<UserScoreModel, decimal>(80).BuildReportTable(data);
Console.WriteLine("Greater than 80");
PrintReport(reportTable);

// Only Jane's score will have BoldProperty.
reportTable = builder.BuildSchema<UserScoreModel, decimal>(90).BuildReportTable(data);
Console.WriteLine("Greater than 90");
PrintReport(reportTable);

void PrintReport(IReportTable<ReportCell> reportTable)
{
    IReportTable<HtmlReportCell> htmlReport = converter.Convert(reportTable);
    string html = writer.WriteToString(htmlReport);
    Console.WriteLine(html);
}

[VerticalReport(PostBuilder = typeof(PostBuilder))]
internal class UserScoreModel
{
    [ReportColumn(1, "Name")]
    public string Name { get; set; }

    [ReportColumn(2, "Score")]
    public decimal Score { get; set; }

    // Need to implement IReportSchemaPostBuilder<TModel, TParameter> interface
    // where TModel is your model type and TParameter is type of parameter
    // that will be passed during building of report schema.
    private class PostBuilder : IReportSchemaPostBuilder<UserScoreModel, decimal>
    {
        public void Build(
            IReportSchemaBuilder<UserScoreModel> builder,
            decimal minScore,
            BuildOptions options)
        {
            BoldProperty boldProperty = new BoldProperty();
            builder.ForColumn("Score")
                .AddDynamicProperties((UserScoreModel m) => m.Score > minScore ? boldProperty : null);
        }
    }
}
