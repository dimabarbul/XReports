using System;
using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_WithCustomHeaderProperty_CorrectProperties()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddHeaderProperties(new CustomHeaderProperty());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
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

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Array.Empty<string>());

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
