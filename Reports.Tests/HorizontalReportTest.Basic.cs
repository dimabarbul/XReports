using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.ValueProviders;
using Xunit;

namespace Reports.Tests
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_TwoRows_CorrectCells()
        {
            HorizontalReportBuilder<(string FirstName, string LastName)> reportBuilder = new HorizontalReportBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddRow("First name", x => x.FirstName);
            reportBuilder.AddRow("Last name", x => x.LastName);

            IReportTable table = reportBuilder.Build(new[]
            {
                ("John", "Doe"),
                ("Jane", "Do"),
            });

            IReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(0);

            IReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].DisplayValue.Should().Be("First name");
            cells[0][1].DisplayValue.Should().Be("John");
            cells[0][2].DisplayValue.Should().Be("Jane");
            cells[1][0].DisplayValue.Should().Be("Last name");
            cells[1][1].DisplayValue.Should().Be("Doe");
            cells[1][2].DisplayValue.Should().Be("Do");
        }

        [Fact]
        public void Build_SequentialNumberValueProvider_CorrectValues()
        {
            HorizontalReportBuilder<string> reportBuilder = new HorizontalReportBuilder<string>();
            reportBuilder.AddRow("#", new SequentialNumberValueProvider());
            reportBuilder.AddRow("Name", s => s);

            IReportTable table = reportBuilder.Build(new[]
            {
                "John Doe",
                "Jane Doe",
            });

            IReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(0);

            IReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].DisplayValue.Should().Be("#");
            cells[0][1].DisplayValue.Should().Be("1");
            cells[0][2].DisplayValue.Should().Be("2");
            cells[1][0].DisplayValue.Should().Be("Name");
            cells[1][1].DisplayValue.Should().Be("John Doe");
            cells[1][2].DisplayValue.Should().Be("Jane Doe");
        }
    }
}
