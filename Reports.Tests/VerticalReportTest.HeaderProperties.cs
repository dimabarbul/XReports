using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
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

            IReportTable<ReportCell> table = reportBuilder.Build(new []
            {
                "Test",
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.HeaderRows);
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

            IReportTable<ReportCell> table = reportBuilder.Build(new string[] { });

            ReportCell[][] cells = this.GetCellsAsArray(table.HeaderRows);
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

            IReportTable<ReportCell> table = reportBuilder.Build(new string[] { });

            ReportCell[][] cells = this.GetCellsAsArray(table.HeaderRows);
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
            public void Process(ReportCell cell)
            {
                if (cell.InternalValue == "Complex")
                {
                    cell.AddProperty(new CustomHeaderProperty());
                }
            }
        }
    }
}
