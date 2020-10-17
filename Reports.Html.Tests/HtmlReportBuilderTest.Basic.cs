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
            ReportTable table = new ReportTable();
            table.Cells = new List<List<IReportCell>>()
            {
                new List<IReportCell>()
                {
                    new HeaderReportCell("Value"),
                },
                new List<IReportCell>()
                {
                    new ReportCell<string>("Test"),
                }
            };

            HtmlReportBuilder builder = new HtmlReportBuilder();
            HtmlReportTable htmlReportTable = builder.Build(table);

            htmlReportTable.Header.Cells.Should().HaveCount(1);
            htmlReportTable.Header.Cells[0][0].Text.Should().Be("Value");
            htmlReportTable.Header.Cells[0][0].ColSpan.Should().Be(1);
            htmlReportTable.Header.Cells[0][0].RowSpan.Should().Be(1);
            htmlReportTable.Body.Cells.Should().HaveCount(1);
            htmlReportTable.Body.Cells[0][0].Text.Should().Be("Test");
            htmlReportTable.Body.Cells[0][0].ColSpan.Should().Be(1);
            htmlReportTable.Body.Cells[0][0].RowSpan.Should().Be(1);
        }
    }
}
