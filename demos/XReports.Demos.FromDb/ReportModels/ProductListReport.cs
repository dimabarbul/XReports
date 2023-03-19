using XReports.Demos.FromDb.XReports.Properties;
using XReports.ReportCellProperties;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.Attributes;
using XReports.SchemaBuilders.ValueProviders;

namespace XReports.Demos.FromDb.ReportModels
{
    [VerticalReport(PostBuilder = typeof(ProductListReportPostBuilder))]
    public class ProductListReport
    {
        [ReportColumn(1, "ID")]
        [Alignment(Alignment.Center, IsHeader = true)]
        public int Id { get; set; }

        [ReportColumn(2, "Title")]
        [Alignment(Alignment.Center, IsHeader = true)]
        public string Title { get; set; }

        [ReportColumn(3, "Description")]
        [Alignment(Alignment.Center, IsHeader = true)]
        public string Description { get; set; }

        [ReportColumn(4, "Price")]
        [Alignment(Alignment.Center, IsHeader = true)]
        [DecimalPrecision(2)]
        [Alignment(Alignment.Center)]
        public decimal? Price { get; set; }

        [ReportColumn(5, "Active")]
        [Alignment(Alignment.Center, IsHeader = true)]
        [CustomProperty(typeof(YesNoProperty))]
        public bool IsActive { get; set; }

        private class ProductListReportPostBuilder : IReportPostBuilder<ProductListReport>
        {
            public void Build(IReportSchemaBuilder<ProductListReport> builder, BuildOptions options)
            {
                builder.InsertColumn(0, "#", new SequentialNumberValueProvider(10000))
                    .AddHeaderProperties(new AlignmentProperty(Alignment.Center));
            }
        }
    }
}
