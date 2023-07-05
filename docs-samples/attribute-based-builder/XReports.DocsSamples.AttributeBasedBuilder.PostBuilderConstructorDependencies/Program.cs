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
    .AddAttributeBasedBuilder(static _ => { }, ServiceLifetime.Scoped)
    .AddReportConverter<HtmlReportCell>(
        c => c.AddFromAssembly(typeof(BoldPropertyHtmlHandler).Assembly))
    .AddHtmlStringWriter()
    // Register service class in DI container so it can be injected.
    .AddScoped<LotteryService>();
ServiceProvider serviceProvider = services.BuildServiceProvider();

IAttributeBasedBuilder builder = serviceProvider.GetRequiredService<IAttributeBasedBuilder>();
IReportConverter<HtmlReportCell> converter = serviceProvider.GetRequiredService<IReportConverter<HtmlReportCell>>();
IHtmlStringWriter writer = serviceProvider.GetRequiredService<IHtmlStringWriter>();

UserModel[] data =
{
    new UserModel() { Name = "John" },
    new UserModel() { Name = "Jane" },
};

// Depending on random number John or Jane will get BoldProperty assigned.
IReportTable<ReportCell> reportTable = builder.BuildSchema<UserModel>().BuildReportTable(data);
IReportTable<HtmlReportCell> htmlReport = converter.Convert(reportTable);
string html = writer.WriteToString(htmlReport);
Console.WriteLine(html);

// Service that our post-builder class depends on.internal
internal class LotteryService
{
    public string GetWinner()
    {
        return new Random().Next(1, 10) % 2 == 0 ? "John" : "Jane";
    }
}

[VerticalReport(PostBuilder = typeof(PostBuilder))]
internal class UserModel
{
    [ReportColumn(1, "Name")]
    public string Name { get; set; }

    private class PostBuilder : IReportSchemaPostBuilder<UserModel>
    {
        private readonly LotteryService lotteryService;

        // Inject dependency in constructor.
        public PostBuilder(LotteryService lotteryService)
        {
            this.lotteryService = lotteryService;
        }

        public void Build(IReportSchemaBuilder<UserModel> builder, BuildOptions options)
        {
            // Use injected service.
            string winner = this.lotteryService.GetWinner();
            BoldProperty boldProperty = new BoldProperty();
            builder.ForColumn(nameof(Name))
                .AddDynamicProperties((UserModel m) => m.Name == winner ? boldProperty : null);
        }
    }
}
