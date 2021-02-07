using XReports.Attributes;
using XReports.Demos.FromDb.XReports.Properties;
using XReports.Enums;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Properties;
using XReports.SchemaBuilders;
using XReports.ValueProviders;

namespace XReports.Demos.FromDb.ReportModels
{
    [VerticalReport(PostBuilder = typeof(ProductListReportPostBuilder))]
    public class ProductListReport
    {
        [ReportVariable(1, "ID")]
        [Alignment(Alignment.Center, IsHeader = true)]
        public int Id { get; set; }

        [ReportVariable(2, "Title")]
        [Alignment(Alignment.Center, IsHeader = true)]
        public string Title { get; set; }

        [ReportVariable(3, "Description")]
        [Alignment(Alignment.Center, IsHeader = true)]
        public string Description { get; set; }

        [ReportVariable(4, "Price")]
        [Alignment(Alignment.Center, IsHeader = true)]
        [DecimalPrecision(2)]
        [Alignment(Alignment.Center)]
        public decimal? Price { get; set; }

        [ReportVariable(5, "Active")]
        [Alignment(Alignment.Center, IsHeader = true)]
        [CustomProperty(typeof(YesNoProperty))]
        public bool IsActive { get; set; }

        private class ProductListReportPostBuilder : IVerticalReportPostBuilder<ProductListReport>
        {
            public void Build(VerticalReportSchemaBuilder<ProductListReport> builder)
            {
                builder.InsertColumn(0, "#", new SequentialNumberValueProvider(10000))
                    .AddHeaderProperties(new AlignmentProperty(Alignment.Center));
            }
        }
    }
}
