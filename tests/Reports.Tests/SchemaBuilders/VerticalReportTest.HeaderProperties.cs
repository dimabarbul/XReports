using FluentAssertions;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Reports.SchemaBuilders;
using Xunit;

namespace Reports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_WithCustomHeaderProperty_CorrectProperties()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddHeaderProperties(new CustomHeaderProperty());

            var schema = reportBuilder.BuildSchema();
            IReportTable<ReportCell> table = schema.BuildReportTable(new []
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
        public void Build_WithCustomPropertyForComplexHeaderUsingAddHeaderProperty_CorrectProperties()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s);
            reportBuilder.AddComplexHeader(0, "Complex", "Value");
            reportBuilder.AddComplexHeaderProperties("Complex", new CustomHeaderProperty());

            var schema = reportBuilder.BuildSchema();
            IReportTable<ReportCell> table = schema.BuildReportTable(new string[] { });

            ReportCell[][] cells = this.GetCellsAsArray(table.HeaderRows);
            cells.Should().HaveCount(2);
            cells[0][0].Properties.Should()
                .HaveCount(1).And
                .ContainItemsAssignableTo<CustomHeaderProperty>();
        }

        private class CustomHeaderProperty : ReportCellProperty
        {
        }
    }
}
