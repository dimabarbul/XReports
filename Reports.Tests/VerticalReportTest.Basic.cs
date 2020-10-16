using FluentAssertions;
using Reports.Builders;
using Reports.Models;
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

            ReportTable table = reportBuilder.Build(new[]
            {
                ("John", "Doe"),
                ("Jane", "Do"),
            });

            table.Cells.Should().HaveCount(3);
            table.Cells[0][0].DisplayValue.Should().Be("First name");
            table.Cells[0][1].DisplayValue.Should().Be("Last name");
            table.Cells[1][0].DisplayValue.Should().Be("John");
            table.Cells[1][1].DisplayValue.Should().Be("Doe");
            table.Cells[2][0].DisplayValue.Should().Be("Jane");
            table.Cells[2][1].DisplayValue.Should().Be("Do");
        }
    }
}
