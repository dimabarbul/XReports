using System.Collections.Generic;
using FluentAssertions;
using Reports.Html.Interfaces;
using Reports.Html.Models;
using Reports.Html.PropertyHandlers.StandardHtml;
using Reports.Interfaces;
using Reports.Models.Cells;
using Reports.Models.Properties;
using Xunit;

namespace Reports.Html.Tests
{
    public partial class HtmlReportConverterTest
    {
        [Fact]
        public void Build_BoldProperty_StrongTag()
        {
            ReportTable table = new ReportTable(
                new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] { new ReportCell<string>("Value"), },
                },
                new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] {
                        new ReportCell<string>("Test", new [] { new BoldProperty() }),
                    }
                }
            );

            HtmlReportConverter converter = new HtmlReportConverter(GetPropertyHandlers());
            HtmlReportTable htmlReportTable = converter.Convert(table);

            HtmlReportTableBodyCell[][] cells = this.GetBodyCellsAsArray(htmlReportTable);
            cells.Should().HaveCount(1);
            cells[0][0].Html.Should().Be("<strong>Test</strong>");
        }

        [Fact]
        public void Build_BoldAndMaxLengthProperty_CorrectOrder()
        {
            ReportTable table = new ReportTable(
                new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] { new ReportCell<string>("Value"), },
                },
                new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] {
                        new ReportCell<string>("Test", new IReportCellProperty[]
                        {
                            new BoldProperty(),
                            new MaxLengthProperty(2),
                        }),
                    }
                }
            );

            HtmlReportConverter converter = new HtmlReportConverter(GetPropertyHandlers());
            HtmlReportTable htmlReportTable = converter.Convert(table);

            HtmlReportTableBodyCell[][] cells = this.GetBodyCellsAsArray(htmlReportTable);
            cells.Should().HaveCount(1);
            cells[0][0].Html.Should().Be("<strong>T…</strong>");
        }

        private static IEnumerable<IHtmlPropertyHandler> GetPropertyHandlers()
        {
            return new IHtmlPropertyHandler[]
            {
                new StandardHtmlBoldPropertyHandler(),
                new StandardHtmlMaxLengthPropertyHandler(),
            };
        }
    }
}
