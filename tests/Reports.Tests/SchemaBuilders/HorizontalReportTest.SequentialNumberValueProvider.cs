using FluentAssertions;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Reports.SchemaBuilders;
using Reports.ValueProviders;
using Xunit;

namespace Reports.Tests.SchemaBuilders
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_SequentialNumberValueProviderWithDefaultStartValue_CorrectValues()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("#", new SequentialNumberValueProvider());

            var schema = reportBuilder.BuildSchema();
            IReportTable<ReportCell> table = schema.BuildReportTable(new[]
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
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("#", new SequentialNumberValueProvider(50));

            var schema = reportBuilder.BuildSchema();
            IReportTable<ReportCell> table = schema.BuildReportTable(new[]
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
