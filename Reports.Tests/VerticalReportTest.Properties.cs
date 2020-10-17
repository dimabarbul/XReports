using FluentAssertions;
using Reports.Builders;
using Reports.Interfaces;
using Reports.Models;
using Xunit;

namespace Reports.Tests
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_WithCustomProperty_CorrectProperties()
        {
            VerticalReportBuilder<string> reportBuilder = new VerticalReportBuilder<string>();
            reportBuilder.AddColumn("Value", s => s);
            reportBuilder.AddColumnProcessor("Value", new CustomPropertyProcessor());

            ReportTable table = reportBuilder.Build(new []
            {
                "Test",
            });

            IReportCell[][] cells = this.GetCellsAsArray(table.Cells);
            cells.Should().HaveCount(1);
            cells[0][0].Properties.Should()
                .HaveCount(1).And
                .ContainSingle(p => p is CustomProperty && (p as CustomProperty).Assigned);
        }

        private class CustomPropertyProcessor : IReportCellProcessor
        {
            public void Process(IReportCell cell)
            {
                cell.AddProperty(new CustomProperty(true));
            }
        }

        private class CustomProperty : IReportCellProperty
        {
            public bool Assigned { get; }

            public CustomProperty(bool assigned)
            {
                this.Assigned = assigned;
            }
        }
    }
}
