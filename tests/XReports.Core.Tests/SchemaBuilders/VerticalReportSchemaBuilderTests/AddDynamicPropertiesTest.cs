using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    public class AddDynamicPropertiesTest
    {
        [Fact]
        public void AddDynamicPropertiesShouldAddProperties()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();
            schemaBuilder.AddColumn(new ComputedValueReportCellsProvider<int, int>("Column", x => x));

            schemaBuilder.AddDynamicProperties(x => x > 0 ? new CustomProperty2() : new CustomProperty1());

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

        [Fact]
        public void AddDynamicPropertiesShouldIgnoreNulls()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();
            schemaBuilder.AddColumn(new ComputedValueReportCellsProvider<int, int>("Column", x => x));

            schemaBuilder.AddDynamicProperties(_ => new ReportCellProperty[]
            {
                new CustomProperty1(),
                new CustomProperty2(),
                null,
            });

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(new[] { 0 });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData(0)
                    {
                        Properties = new ReportCellProperty[]
                        {
                            new CustomProperty1(),
                            new CustomProperty2(),
                        },
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
