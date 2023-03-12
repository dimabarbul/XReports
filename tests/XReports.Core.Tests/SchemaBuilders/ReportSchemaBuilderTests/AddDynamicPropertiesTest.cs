using XReports.Extensions;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellsProviders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    /// <seealso cref="ReportColumnBuilderTests.AddDynamicPropertiesTest"/>
    public class AddDynamicPropertiesTest
    {
        [Fact]
        public void AddDynamicPropertiesShouldAddProperties()
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();
            IReportColumnBuilder<int> cellsProviderBuilder =
                schemaBuilder.AddColumn("Column", new ComputedValueReportCellsProvider<int, int>(x => x));

            cellsProviderBuilder.AddDynamicProperties(x => x > 0 ? (ReportCellProperty)new CustomProperty2() : new CustomProperty1());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(new[] { 0, 1 });
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

        [Fact]
        public void AddDynamicPropertiesShouldAddPropertiesForHorizontal()
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();
            IReportColumnBuilder<int> cellsProviderBuilder =
                schemaBuilder.AddColumn("Column", new ComputedValueReportCellsProvider<int, int>(x => x));

            cellsProviderBuilder.AddDynamicProperties(x => x > 0 ? (ReportCellProperty)new CustomProperty2() : new CustomProperty1());

            IReportTable<ReportCell> table = schemaBuilder.BuildHorizontalSchema(0).BuildReportTable(new[] { 0, 1 });
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column"),
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
