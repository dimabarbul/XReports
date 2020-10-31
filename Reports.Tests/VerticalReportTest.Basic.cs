using FluentAssertions;
using Reports.Builders;
using Reports.Interfaces;
using Reports.ValueProviders;
using Xunit;

namespace Reports.Tests
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_TwoRows_CorrectCells()
        {
            VerticalReportBuilder<(string FirstName, string LastName)> reportBuilder = new VerticalReportBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddColumn("First name", x => x.FirstName);
            reportBuilder.AddColumn("Last name", x => x.LastName);

            IReportTable table = reportBuilder.Build(new[]
            {
                ("John", "Doe"),
                ("Jane", "Do"),
            });

            IReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].DisplayValue.Should().Be("First name");
            headerCells[0][1].DisplayValue.Should().Be("Last name");

            IReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].DisplayValue.Should().Be("John");
            cells[0][1].DisplayValue.Should().Be("Doe");
            cells[1][0].DisplayValue.Should().Be("Jane");
            cells[1][1].DisplayValue.Should().Be("Do");
        }

        [Fact]
        public void Build_SequentialNumberValueProvider_CorrectValues()
        {
            VerticalReportBuilder<string> reportBuilder = new VerticalReportBuilder<string>();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider());
            reportBuilder.AddColumn("Name", s => s);

            IReportTable table = reportBuilder.Build(new[]
            {
                "John Doe",
                "Jane Doe",
            });

            IReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].DisplayValue.Should().Be("#");
            headerCells[0][1].DisplayValue.Should().Be("Name");

            IReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].DisplayValue.Should().Be("1");
            cells[0][1].DisplayValue.Should().Be("John Doe");
            cells[1][0].DisplayValue.Should().Be("2");
            cells[1][1].DisplayValue.Should().Be("Jane Doe");
        }
    }
}
