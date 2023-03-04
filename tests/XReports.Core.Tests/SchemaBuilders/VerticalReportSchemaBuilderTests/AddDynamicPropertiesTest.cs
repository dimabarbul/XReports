using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
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
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column"),
                },
            });
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(0, new CustomProperty1()),
                },
                new[]
                {
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
