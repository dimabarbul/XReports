using System.Globalization;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddGlobalPropertiesTest"/>
    public class AddGlobalPropertiesTest
    {
        [Fact]
        public void AddGlobalPropertiesShouldAddPropertiesToAllColumnsAndRows()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();
            schemaBuilder.AddColumn("Value", new ComputedValueReportCellsProvider<int, int>(x => x));
            schemaBuilder.AddColumn("As string", new ComputedValueReportCellsProvider<int, string>(x => x.ToString(CultureInfo.InvariantCulture)));

            schemaBuilder.AddGlobalProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(new[]
            {
                1,
                2,
            });
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] {"Value", "As string" },
            });
            ReportCellProperty[] expectedProperties =
            {
                new CustomProperty1(),
                new CustomProperty2(),
            };
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    new ReportCellData(1)
                    {
                        Properties = expectedProperties,
                    },
                    new ReportCellData("1")
                    {
                        Properties = expectedProperties,
                    },
                },
                new object[]
                {
                    new ReportCellData(2)
                    {
                        Properties = expectedProperties,
                    },
                    new ReportCellData("2")
                    {
                        Properties = expectedProperties,
                    },
                },
            });
        }

        private class CustomProperty1 : ReportCellProperty
        {
            public bool Value { get; init; }
        }

        private class CustomProperty2 : ReportCellProperty
        {
            public bool Value { get; init; }
        }
    }
}
