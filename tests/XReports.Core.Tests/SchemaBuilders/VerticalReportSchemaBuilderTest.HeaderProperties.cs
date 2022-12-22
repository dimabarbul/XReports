using System;
using System.Linq;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders
{
    public partial class VerticalReportSchemaBuilderTest
    {
        [Fact]
        public void BuildShouldApplyCustomHeaderProperty()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddHeaderProperties(new CustomHeaderProperty());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<string>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[]
                {
                    new ReportCellData("Value")
                    {
                        Properties = new[] { new CustomHeaderProperty() },
                    },
                },
            });
        }

        [Fact]
        public void BuildShouldApplyCustomPropertyForComplexHeaderUsingHeaderName()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s);
            reportBuilder.AddComplexHeader(0, "Complex", "Value");
            reportBuilder.AddComplexHeaderProperties("Complex", new CustomHeaderProperty());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Array.Empty<string>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Complex")
                    {
                        Properties = new[] { new CustomHeaderProperty() },
                    },
                },
                new object[] { "Value" },
            });
        }

        private class CustomHeaderProperty : ReportCellProperty
        {
        }
    }
}
