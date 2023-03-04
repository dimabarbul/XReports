using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddHeaderPropertiesTest"/>
    public class AddHeaderPropertiesTest
    {
        [Fact]
        public void AddHeaderPropertiesShouldAddCustomHeaderProperties()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s)
                .AddHeaderProperties(new CustomHeaderProperty1(), new CustomHeaderProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });

            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(
                        "Value", new CustomHeaderProperty1(), new CustomHeaderProperty2()),
                    ReportCellHelper.CreateReportCell("Test"),
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
