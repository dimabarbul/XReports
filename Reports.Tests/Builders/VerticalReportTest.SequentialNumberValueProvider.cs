using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Reports.ValueProviders;
using Xunit;

namespace Reports.Tests.Builders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_SequentialNumberValueProviderWithDefaultStartValue_CorrectValues()
        {
            VerticalReportBuilder<string> reportBuilder = new VerticalReportBuilder<string>();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider());

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                "John Doe",
                "Jane Doe",
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("#");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<int>().Should().Be(1);
            cells[1][0].GetValue<int>().Should().Be(2);
        }

        [Fact]
        public void Build_SequentialNumberValueProviderWithNonDefaultStartValue_CorrectValues()
        {
            VerticalReportBuilder<string> reportBuilder = new VerticalReportBuilder<string>();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider(15));

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                "John Doe",
                "Jane Doe",
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("#");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<int>().Should().Be(15);
            cells[1][0].GetValue<int>().Should().Be(16);
        }
    }
}
