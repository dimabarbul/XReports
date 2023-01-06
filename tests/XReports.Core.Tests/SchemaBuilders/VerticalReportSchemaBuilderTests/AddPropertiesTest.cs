using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddPropertiesTest"/>
    public class AddPropertiesTest
    {
        [Fact]
        public void AddPropertiesShouldAddPropertiesToAllRows()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
                "Test2",
            });

            ReportCellProperty[] expectedProperties = { new CustomProperty1(), new CustomProperty2() };
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
                        Properties = expectedProperties,
                    },
                },
                new[]
                {
                    new ReportCellData("Test2")
                    {
                        Properties = expectedProperties,
                    },
                },
            });
        }

        private class CustomProperty1 : ReportCellProperty
        {
        }

        private class CustomProperty2 : ReportCellProperty
        {
        }
    }
}
