using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddPropertiesTest"/>
    public class AddPropertiesTest
    {
        [Fact]
        public void AddPropertiesShouldAddPropertiesToAllColumns()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s)
                .AddProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
                "Test2",
            });

            ReportCellProperty[] expectedProperties = { new CustomProperty1(), new CustomProperty2() };
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    "Value",
                    new ReportCellData("Test")
                    {
                        Properties = expectedProperties,
                    },
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
