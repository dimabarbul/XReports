using Xunit;

namespace XReports.Html.Tests
{
    public partial class HtmlReportConverterTest
    {
        [Fact(Skip = "Need to move to properties tests")]
        public void Build_BoldProperty_StrongTag()
        {
            // IReportTable<ReportCell> table = new ReportTable<ReportCell>()
            // {
            //     HeaderRows = new List<IEnumerable<ReportCell>>()
            //     {
            //         new ReportCell[] { this.CreateReportCell("Value"), },
            //     },
            //     Rows = new List<IEnumerable<ReportCell>>()
            //     {
            //         new ReportCell[] {
            //             this.CreateReportCell("Test", new BoldProperty()),
            //         }
            //     }
            // };
            //
            // ReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>(GetPropertyHandlers());
            // IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(table);
            //
            // HtmlReportCell[][] cells = this.GetBodyCellsAsArray(htmlReportTable);
            // cells.Should().HaveCount(1);
            // cells[0][0].Html.Should().Be("<strong>Test</strong>");
        }

        [Fact(Skip = "Need to move to properties tests")]
        public void Build_BoldAndMaxLengthProperty_CorrectOrder()
        {
            // IReportTable<ReportCell> table = new ReportTable<ReportCell>()
            // {
            //     HeaderRows = new List<IEnumerable<ReportCell>>()
            //     {
            //         new ReportCell[] { new ReportCell<string>("Value"), },
            //     },
            //     Rows = new List<IEnumerable<ReportCell>>()
            //     {
            //         new ReportCell[]
            //         {
            //             this.CreateReportCell("Test", new BoldProperty(), new MaxLengthProperty(2)),
            //         }
            //     }
            // };
            //
            // ReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>(GetPropertyHandlers());
            // IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(table);
            //
            // HtmlReportCell[][] cells = this.GetBodyCellsAsArray(htmlReportTable);
            // cells.Should().HaveCount(1);
            // cells[0][0].Html.Should().Be("<strong>T…</strong>");
        }

        // private static IEnumerable<IPropertyHandler<HtmlReportCell>> GetPropertyHandlers()
        // {
        //     return new IPropertyHandler<HtmlReportCell>[]
        //     {
        //         new StandardHtmlBoldPropertyHandler(),
        //         new StandardHtmlMaxLengthPropertyHandler(),
        //     };
        // }
    }
}
