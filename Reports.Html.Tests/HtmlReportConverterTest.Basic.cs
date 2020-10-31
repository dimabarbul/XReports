using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Reports.Html.Interfaces;
using Reports.Html.Models;
using Reports.Interfaces;
using Reports.Models;
using Xunit;

namespace Reports.Html.Tests
{
    public partial class HtmlReportConverterTest
    {
        [Fact]
        public void Build_TextValue_CorrectCells()
        {
            IReportTable table = new ReportTable(
                new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] { new ReportCell<string>("Value"), },
                },
                new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] { new ReportCell<string>("Test"), }
                }
            );

            HtmlReportConverter converter = new HtmlReportConverter(Enumerable.Empty<IHtmlPropertyHandler>());
            HtmlReportTable htmlReportTable = converter.Convert(table);

            HtmlReportTableHeaderCell[][] headerCells = this.GetHeaderCellsAsArray(htmlReportTable);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].Text.Should().Be("Value");
            headerCells[0][0].ColSpan.Should().Be(1);
            headerCells[0][0].RowSpan.Should().Be(1);

            HtmlReportTableBodyCell[][] cells = this.GetBodyCellsAsArray(htmlReportTable);
            cells.Should().HaveCount(1);
            cells[0][0].Text.Should().Be("Test");
            cells[0][0].ColSpan.Should().Be(1);
            cells[0][0].RowSpan.Should().Be(1);
        }
    }
}
