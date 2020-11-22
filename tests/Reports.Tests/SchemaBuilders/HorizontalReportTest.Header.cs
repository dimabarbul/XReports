using FluentAssertions;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Reports.SchemaBuilders;
using Xunit;

namespace Reports.Tests.SchemaBuilders
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_WithHeader_CorrectValues()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddHeaderRow(0, string.Empty, s => s.Length);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new []
            {
                "Test",
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be(string.Empty);
            headerCells[0][1].GetValue<int>().Should().Be(4);
        }

        [Fact]
        public void Build_WithSeveralHeaders_CorrectValues()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddHeaderRow(0, string.Empty, s => s.Length);
            reportBuilder.AddHeaderRow(1, string.Empty, s => s.Substring(0, 1));

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new []
            {
                "Test",
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(2);
            headerCells[0][0].GetValue<string>().Should().Be(string.Empty);
            headerCells[0][1].GetValue<int>().Should().Be(4);
            headerCells[1][0].GetValue<string>().Should().Be(string.Empty);
            headerCells[1][1].GetValue<string>().Should().Be("T");
        }
    }
}
