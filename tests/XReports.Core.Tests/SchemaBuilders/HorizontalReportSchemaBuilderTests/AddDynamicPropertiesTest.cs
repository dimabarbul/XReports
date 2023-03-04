using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddDynamicPropertiesTest"/>
    public class AddDynamicPropertiesTest
    {
        [Fact]
        public void AddDynamicPropertiesShouldAddProperties()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();
            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder =
                schemaBuilder.AddRow("Row", new ComputedValueReportCellsProvider<int, int>(x => x));

            cellsProviderBuilder.AddDynamicProperties(x => x > 0 ? (ReportCellProperty)new CustomProperty2() : new CustomProperty1());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(new[] { 0, 1 });
            table.HeaderRows.Should().BeEmpty();
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Row"),
                    ReportCellHelper.CreateReportCell(0, new CustomProperty1()),
                    ReportCellHelper.CreateReportCell(1, new CustomProperty2()),
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
