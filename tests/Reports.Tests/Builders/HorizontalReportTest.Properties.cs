using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Xunit;

namespace Reports.Tests.Builders
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void Build_WithCustomProperty_CorrectProperties()
        {
            HorizontalReportBuilder<string> reportBuilder = new HorizontalReportBuilder<string>();
            reportBuilder.AddRow("Value", s => s)
                .AddProcessor(new CustomPropertyProcessor());

            IReportTable<ReportCell> table = reportBuilder.Build(new []
            {
                "Test",
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
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
                .AddProperty(new CustomProperty());

            IReportTable<ReportCell> table = reportBuilder.Build(new []
            {
                "Test",
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].Properties.Should().BeEmpty();
            cells[0][1].Properties.Should()
                .HaveCount(1).And
                .AllBeOfType<CustomProperty>();
        }

        private class CustomPropertyProcessor : IReportCellProcessor
        {
            public void Process(ReportCell cell)
            {
                cell.AddProperty(new CustomProperty(true));
            }
        }

        private class CustomProperty : ReportCellProperty
        {
            public bool Assigned { get; }

            public CustomProperty(bool assigned = false)
            {
                this.Assigned = assigned;
            }
        }
    }
}
