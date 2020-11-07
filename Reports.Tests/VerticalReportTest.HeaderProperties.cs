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
        public void Build_WithCustomHeaderProperty_CorrectProperties()
        {
            VerticalReportBuilder<string> reportBuilder = new VerticalReportBuilder<string>();
            reportBuilder.AddColumn("Value", s => s);
            reportBuilder.AddHeaderProperty("Value", new CustomHeaderProperty());

            IReportTable table = reportBuilder.Build(new []
            {
                "Test",
            });

            IReportCell[][] cells = this.GetCellsAsArray(table.HeaderRows);
            cells.Should().HaveCount(1);
            cells[0][0].Properties.Should()
                .HaveCount(1).And
                .ContainItemsAssignableTo<CustomHeaderProperty>();
        }

        [Fact]
        public void Build_WithCustomPropertyForComplexHeaderUsingAddProperty_CorrectProperties()
        {
            VerticalReportBuilder<string> reportBuilder = new VerticalReportBuilder<string>();
            reportBuilder.AddColumn("Value", s => s);
            reportBuilder.AddComplexHeader(0, "Complex", "Value");
            reportBuilder.AddHeaderProperty("Complex", new CustomHeaderProperty());

            IReportTable table = reportBuilder.Build(new string[] { });

            IReportCell[][] cells = this.GetCellsAsArray(table.HeaderRows);
            cells.Should().HaveCount(2);
            cells[0][0].Properties.Should()
                .HaveCount(1).And
                .ContainItemsAssignableTo<CustomHeaderProperty>();
        }

        [Fact]
        public void Build_WithCustomPropertyForComplexHeaderUsingProcessor_CorrectProperties()
        {
            VerticalReportBuilder<string> reportBuilder = new VerticalReportBuilder<string>();
            reportBuilder.AddColumn("Value", s => s);
            reportBuilder.AddComplexHeader(0, "Complex", "Value");
            reportBuilder.AddHeaderCellProcessor(new CustomHeaderPropertyProcessor());

            IReportTable table = reportBuilder.Build(new string[] { });

            IReportCell[][] cells = this.GetCellsAsArray(table.HeaderRows);
            cells.Should().HaveCount(2);
            cells[0][0].Properties.Should()
                .HaveCount(1).And
                .ContainItemsAssignableTo<CustomHeaderProperty>();
        }

        private class CustomHeaderProperty : IReportCellProperty
        {
        }

        private class CustomHeaderPropertyProcessor : IReportCellProcessor
        {
            public void Process(IReportCell cell)
            {
                if (cell.DisplayValue == "Complex")
                {
                    cell.AddProperty(new CustomHeaderProperty());
                }
            }
        }
    }
}
