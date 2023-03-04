using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddPropertiesTest"/>
    public class AddHeaderRowPropertiesTest
    {
        [Fact]
        public void AddHeaderRowPropertiesShouldAddPropertiesToAllColumns()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddHeaderRow("Header", s => s.ToUpperInvariant())
                .AddProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
                "Test2",
            });

            ReportCellProperty[] expectedProperties = { new CustomProperty1(), new CustomProperty2() };
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Header"),
                    ReportCellHelper.CreateReportCell("TEST", expectedProperties),
                    ReportCellHelper.CreateReportCell("TEST2", expectedProperties),
                },
            });
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                    ReportCellHelper.CreateReportCell("Test"),
                    ReportCellHelper.CreateReportCell("Test2"),
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
