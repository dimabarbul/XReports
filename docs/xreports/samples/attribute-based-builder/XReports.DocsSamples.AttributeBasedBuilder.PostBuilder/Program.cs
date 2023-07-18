using XReports.Converter;
using XReports.Html;
using XReports.Html.PropertyHandlers;
using XReports.Html.Writers;
using XReports.ReportCellProperties;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.Attributes;
using XReports.Table;

ReportModel[] data =
{
    new ReportModel()
    {
        Name = "John Doe",
        Email = "johndoe@example.com",
        Score = 9,
    },
    new ReportModel()
    {
        Name = "Jane Doe",
        Email = "janedoe@example.com",
        Score = 10,
    },
};

AttributeBasedBuilder builder = new AttributeBasedBuilder(Array.Empty<IAttributeHandler>());
IReportSchema<ReportModel> schema = builder.BuildSchema<ReportModel>();
ReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>(new[]
{
    new BoldPropertyHtmlHandler(),
});
IReportTable<HtmlReportCell> htmlReport = converter.Convert(schema.BuildReportTable(data));
HtmlStringWriter writer = new HtmlStringWriter(new HtmlStringCellWriter());
string html = writer.Write(htmlReport);

Console.WriteLine(html);

[HorizontalReport(PostBuilder = typeof(PostBuilder))]
internal class ReportModel
{
    [HeaderRow]
    [ReportColumn(1)]
    public string Name { get; set; }

    [ReportColumn(1)]
    public string Email { get; set; }

    [ReportColumn(2)]
    [DecimalPrecision(2)]
    public decimal Score { get; set; }

    // Post-builder class does NOT have to be nested class.
    private class PostBuilder : IReportSchemaPostBuilder<ReportModel>
    {
        // This method will be called after all columns/rows are added to schema builder.
        public void Build(IReportSchemaBuilder<ReportModel> builder, BuildOptions options)
        {
            BoldProperty boldProperty = new BoldProperty();
            builder.ForColumn(new ColumnId(nameof(Score)))
                .AddDynamicProperties(m => m.Score > 9 ? boldProperty : null);
        }
    }
}
