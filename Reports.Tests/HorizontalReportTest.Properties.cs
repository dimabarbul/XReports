using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models.Properties;
using Xunit;

namespace Reports.Tests
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_WithCustomProperty_CorrectProperties()
        {
            HorizontalReportBuilder<string> reportBuilder = new HorizontalReportBuilder<string>();
            reportBuilder.AddRow("Value", s => s)
                .AddProcessor(new CustomPropertyProcessor());

            IReportTable table = reportBuilder.Build(new []
            {
                "Test",
            });

            IReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].Properties.Should().BeEmpty();
            cells[0][1].Properties.Should()
                .HaveCount(1).And
                .ContainSingle(p => p is CustomProperty && ((CustomProperty) p).Assigned);
        }

        [Fact]
        public void Build_CustomPropertyWithoutProcessor_CorrectProperties()
        {
            HorizontalReportBuilder<string> reportBuilder = new HorizontalReportBuilder<string>();
            reportBuilder.AddRow("Value", s => s)
                .AddProperty(new BoldProperty());

            IReportTable table = reportBuilder.Build(new []
            {
                "Test",
            });

            IReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].Properties.Should().BeEmpty();
            cells[0][1].Properties.Should()
                .HaveCount(1).And
                .AllBeOfType<BoldProperty>();
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