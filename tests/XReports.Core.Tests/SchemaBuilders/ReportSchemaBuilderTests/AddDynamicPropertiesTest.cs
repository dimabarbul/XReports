using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellProviders;
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
            IReportColumnBuilder<int> columnBuilder =
                schemaBuilder.AddColumn("Column", new ComputedValueReportCellProvider<int, int>(x => x));

            columnBuilder.AddDynamicProperties(x => x > 0 ? (IReportCellProperty)new CustomProperty2() : new CustomProperty1());

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
            IReportColumnBuilder<int> columnBuilder =
                schemaBuilder.AddColumn("Column", new ComputedValueReportCellProvider<int, int>(x => x));

            columnBuilder.AddDynamicProperties(x => x > 0 ? (IReportCellProperty)new CustomProperty2() : new CustomProperty1());

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

        private class CustomProperty1 : IReportCellProperty
        {
        }

        private class CustomProperty2 : IReportCellProperty
        {
        }
    }
}
