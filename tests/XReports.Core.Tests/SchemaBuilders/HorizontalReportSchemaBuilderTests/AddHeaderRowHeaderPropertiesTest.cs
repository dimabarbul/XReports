using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddHeaderPropertiesTest"/>
    public class AddHeaderRowHeaderPropertiesTest
    {
        [Fact]
        public void AddHeaderRowHeaderPropertiesShouldAddCustomHeaderProperties()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddHeaderRow("Header", s => s.ToUpperInvariant())
                .AddHeaderProperties(new CustomHeaderProperty1(), new CustomHeaderProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData("Header")
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new CustomHeaderProperty1(),
                            new CustomHeaderProperty2(),
                        },
                    },
                    "TEST",
                },
            });
        }

        private class CustomHeaderProperty1 : ReportCellProperty
        {
        }

        private class CustomHeaderProperty2 : ReportCellProperty
        {
        }
    }
}
