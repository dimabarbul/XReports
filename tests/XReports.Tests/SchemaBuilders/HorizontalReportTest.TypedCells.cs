using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void BuildShouldSupportInteger()
        {
            HorizontalReportSchemaBuilder<int> reportBuilder = new HorizontalReportSchemaBuilder<int>();
            reportBuilder.AddRow("#", i => i);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                3,
                6,
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().BeEmpty();

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].ValueType.Should().Be(typeof(string));
            cells[0][0].GetValue<string>().Should().Be("#");
            cells[0][1].ValueType.Should().Be(typeof(int));
            cells[0][1].GetValue<int>().Should().Be(3);
            cells[0][2].ValueType.Should().Be(typeof(int));
            cells[0][2].GetValue<int>().Should().Be(6);
        }
    }
}
