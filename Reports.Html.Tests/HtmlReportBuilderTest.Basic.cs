using System.Collections.Generic;
using FluentAssertions;
using Reports.Html.Models;
using Reports.Interfaces;
using Reports.Models;
using Reports.Models.Cells;
using Xunit;

namespace Reports.Html.Tests
{
    public partial class HtmlReportBuilderTest
    {
        [Fact]
        public void Build_TextValue_CorrectCells()
        {
            ReportTable table = new ReportTable
            {
                HeaderCells = new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] { new ReportCell<string>("Value"), },
                },
                Cells = new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] { new ReportCell<string>("Test"), }
                },
            };

            HtmlReportBuilder builder = new HtmlReportBuilder();
            HtmlReportTable htmlReportTable = builder.Build(table);

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
