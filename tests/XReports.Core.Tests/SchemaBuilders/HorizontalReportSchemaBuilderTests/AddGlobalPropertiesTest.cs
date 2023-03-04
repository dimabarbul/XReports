using System.Globalization;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    /// <seealso cref="XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests.AddGlobalPropertiesTest"/>
    public class AddGlobalPropertiesTest
    {
        [Fact]
        public void AddGlobalPropertiesShouldAddPropertiesToAllColumnsAndRows()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();
            schemaBuilder.AddRow("Value", new ComputedValueReportCellsProvider<int, int>(x => x));
            schemaBuilder.AddRow("As string", new ComputedValueReportCellsProvider<int, string>(x => x.ToString(CultureInfo.InvariantCulture)));

            schemaBuilder.AddGlobalProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(new[]
            {
                1,
                2,
            });
            ReportCellProperty[] expectedProperties =
            {
                new CustomProperty1(),
                new CustomProperty2(),
            };
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                    ReportCellHelper.CreateReportCell(1, expectedProperties),
                    ReportCellHelper.CreateReportCell(2, expectedProperties),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("As string"),
                    ReportCellHelper.CreateReportCell("1", expectedProperties),
                    ReportCellHelper.CreateReportCell("2", expectedProperties),
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
