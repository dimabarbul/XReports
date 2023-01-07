using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddDynamicPropertiesTest"/>
    public class AddDynamicPropertiesTest
    {
        [Fact]
        public void AddDynamicPropertiesShouldAddProperties()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();
            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder =
                schemaBuilder.AddColumn("Column", new ComputedValueReportCellsProvider<int, int>(x => x));

            cellsProviderBuilder.AddDynamicProperties(x => x > 0 ? (ReportCellProperty)new CustomProperty2() : new CustomProperty1());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(new[] { 0, 1 });
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Column" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData(0)
                    {
                        Properties = new[] { new CustomProperty1() },
                    },
                },
                new object[]
                {
                    new ReportCellData(1)
                    {
                        Properties = new[] { new CustomProperty2() },
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
