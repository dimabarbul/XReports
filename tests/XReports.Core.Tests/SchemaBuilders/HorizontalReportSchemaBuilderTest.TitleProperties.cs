using System;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders
{
    public partial class HorizontalReportSchemaBuilderTest
    {
        [Fact]
        public void BuildShouldSupportCustomHeaderProperty()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new();
            reportBuilder.AddRow("Value", s => s)
                .AddHeaderProperties(new CustomTitleProperty());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });

            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Value")
                    {
                        Properties = new[] { new CustomTitleProperty() },
                    },
                    "Test",
                },
            });
        }

        [Fact]
        public void BuildShouldSupportCustomPropertyForComplexHeaderUsingHeaderName()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddComplexHeader(0, "Complex", "Value");
            reportBuilder.AddComplexHeaderProperties("Complex", new CustomTitleProperty());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Array.Empty<string>());

            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Complex")
                    {
                        Properties = new[] { new CustomTitleProperty() },
                    },
                    "Value",
                },
            });
        }

        private class CustomTitleProperty : ReportCellProperty
        {
        }
    }
}
