using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Reports.ValueProviders;
using Xunit;

namespace Reports.Tests.Builders
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_TwoRows_CorrectCells()
        {
            HorizontalReportBuilder<(string FirstName, string LastName)> reportBuilder = new HorizontalReportBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddRow("First name", x => x.FirstName);
            reportBuilder.AddRow("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                ("John", "Doe"),
                ("Jane", "Do"),
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().BeEmpty();

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<string>().Should().Be("First name");
            cells[0][1].GetValue<string>().Should().Be("John");
            cells[0][2].GetValue<string>().Should().Be("Jane");
            cells[1][0].GetValue<string>().Should().Be("Last name");
            cells[1][1].GetValue<string>().Should().Be("Doe");
            cells[1][2].GetValue<string>().Should().Be("Do");
        }
    }
}
