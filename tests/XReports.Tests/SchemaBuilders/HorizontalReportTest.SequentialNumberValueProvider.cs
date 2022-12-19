using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.ValueProviders;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void BuildShouldSupportSequentialNumberValueProviderWithDefaultStartValue()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("#", new SequentialNumberValueProvider());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
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
        public void BuildShouldSupportSequentialNumberValueProviderWithNonDefaultStartValue()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("#", new SequentialNumberValueProvider(50));

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
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
