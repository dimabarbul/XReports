using XReports.Extensions;
using XReports.SchemaBuilders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddHeaderPropertiesTest"/>
    public class AddHeaderPropertiesTest
    {
        [Fact]
        public void AddHeaderPropertiesShouldAddCustomHeaderProperties()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddHeaderProperties(new CustomHeaderProperty1(), new CustomHeaderProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildVerticalSchema().BuildReportTable(new[]
            {
                "Test",
            });

            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(
                        "Value", new CustomHeaderProperty1(), new CustomHeaderProperty2()),
                },
            });
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Test"),
                },
            });
        }

        [Fact]
        public void AddHeaderPropertiesShouldAddCustomHeaderPropertiesForHorizontal()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddHeaderProperties(new CustomHeaderProperty1(), new CustomHeaderProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildHorizontalSchema(0).BuildReportTable(new[]
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
