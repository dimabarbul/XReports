using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void BuildShouldApplyCustomPropertyUsingProcessor()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new();
            reportBuilder.AddColumn("Value", s => s)
                .AddProcessors(new CustomPropertyProcessor());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Value" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[]
                {
                    new ReportCellData("Test")
                    {
                        Properties = new[] { new CustomProperty(true) },
                    },
                },
            });
        }

        [Fact]
        public void BuildShouldApplyCustomPropertyUsingAddPropertiesMethod()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new();
            reportBuilder.AddColumn("Value", s => s)
                .AddProperties(new CustomProperty(true));

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Value" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[]
                {
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
