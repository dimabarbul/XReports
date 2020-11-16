using System.Linq;
using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Xunit;

namespace Reports.Tests.Builders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_WithHeaderCellProcessor_HeaderProcessed()
        {
            VerticalReportBuilder<int> reportBuilder = new VerticalReportBuilder<int>();
            reportBuilder.AddColumn("#", i => i);
            reportBuilder.AddHeaderCellProcessor(new CustomHeaderCellProcessor());

            IReportTable<ReportCell> table = reportBuilder.Build(Enumerable.Empty<int>());

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("-- # --");
        }

        public class CustomHeaderCellProcessor : IReportCellProcessor<int>
        {
            public void Process(ReportCell cell, int data)
            {
                cell.InternalValue = $"-- {cell.InternalValue} --";
            }
        }
    }
}
