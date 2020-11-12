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
        public void Build_SequentialNumberValueProviderWithDefaultStartValue_CorrectValues()
        {
            HorizontalReportBuilder<string> reportBuilder = new HorizontalReportBuilder<string>();
            reportBuilder.AddRow("#", new SequentialNumberValueProvider());

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                "John Doe",
                "Jane Doe",
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().BeEmpty();

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].GetValue<string>().Should().Be("#");
            cells[0][1].GetValue<int>().Should().Be(1);
            cells[0][2].GetValue<int>().Should().Be(2);
        }

        [Fact]
        public void Build_SequentialNumberValueProviderWithNonDefaultStartValue_CorrectValues()
        {
            HorizontalReportBuilder<string> reportBuilder = new HorizontalReportBuilder<string>();
            reportBuilder.AddRow("#", new SequentialNumberValueProvider(50));

            IReportTable<ReportCell> table = reportBuilder.Build(new[]
            {
                "John Doe",
                "Jane Doe",
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].GetValue<string>().Should().Be("#");
            cells[0][1].GetValue<int>().Should().Be(50);
            cells[0][2].GetValue<int>().Should().Be(51);
        }
    }
}
