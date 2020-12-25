using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
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
            IReportTable<ReportCell> table = new ReportTable<ReportCell>()
            {
                HeaderRows = new List<IEnumerable<ReportCell>>()
                {
                    new ReportCell[] { new ReportCell<string>("Value"), },
                },
                Rows = new List<IEnumerable<ReportCell>>()
                {
                    new ReportCell[] { new ReportCell<string>("Test"), }
                }
            };

            ReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>(Enumerable.Empty<IPropertyHandler<HtmlReportCell>>());
            IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(table);

            HtmlReportCell[][] headerCells = this.GetHeaderCellsAsArray(htmlReportTable);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].Html.Should().Be("Value");
            headerCells[0][0].ColumnSpan.Should().Be(1);
            headerCells[0][0].RowSpan.Should().Be(1);

            HtmlReportCell[][] cells = this.GetBodyCellsAsArray(htmlReportTable);
            cells.Should().HaveCount(1);
            cells[0][0].Html.Should().Be("Test");
            cells[0][0].ColumnSpan.Should().Be(1);
            cells[0][0].RowSpan.Should().Be(1);
        }
    }
}
