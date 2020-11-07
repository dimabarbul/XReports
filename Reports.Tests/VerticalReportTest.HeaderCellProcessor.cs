using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Xunit;

namespace Reports.Tests
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_WithHeaderCellProcessor_HeaderProcessed()
        {
            VerticalReportBuilder<int> reportBuilder = new VerticalReportBuilder<int>();
            reportBuilder.AddColumn("#", i => i);
            reportBuilder.AddHeaderCellProcessor(new CustomHeaderCellProcessor());

            IReportTable table = reportBuilder.Build(new int[] { });

            IReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].DisplayValue.Should().Be("<em>#</em>");
            headerCells[0][0].ValueType.Should().Be(typeof(string));
        }

        public class CustomHeaderCellProcessor : IReportCellProcessor
        {
            public void Process(IReportCell cell)
            {
                cell.DisplayValue = $"<em>{cell.DisplayValue}</em>";
            }
        }
    }
}
