using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddHeaderPropertiesTest"/>
    public class AddHeaderPropertiesTest
    {
        [Fact]
        public void AddHeaderPropertiesShouldAddCustomHeaderProperties()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddHeaderProperties(new CustomHeaderProperty1(), new CustomHeaderProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[]
                {
                    new ReportCellData("Value")
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new CustomHeaderProperty1(),
                            new CustomHeaderProperty2(),
                        },
                    },
                },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "Test" },
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
