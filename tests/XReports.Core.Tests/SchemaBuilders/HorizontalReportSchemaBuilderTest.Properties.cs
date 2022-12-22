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
        public void BuildShouldApplyCustomPropertyWithProcessor()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s)
                .AddProcessors(new CustomPropertyProcessor());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });

            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    "Value",
                    new ReportCellData("Test")
                    {
                        Properties = new[] { new CustomProperty(true) },
                    },
                },
            });
        }

        [Fact]
        public void BuildShouldApplyCustomPropertyWithoutProcessor()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s)
                .AddProperties(new CustomProperty());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });

            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    "Value",
                    new ReportCellData("Test")
                    {
                        Properties = new[] { new CustomProperty(true) },
                    },
                },
            });
        }

        private class CustomPropertyProcessor : IReportCellProcessor<string>
        {
            public void Process(ReportCell cell, string data)
            {
                cell.AddProperty(new CustomProperty(true));
            }
        }

        private class CustomProperty : ReportCellProperty
        {
            public CustomProperty(bool assigned = false)
            {
                this.Assigned = assigned;
            }

            public bool Assigned { get; }
        }
    }
}
