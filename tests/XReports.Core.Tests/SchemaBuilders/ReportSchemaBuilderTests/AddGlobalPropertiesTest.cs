using System.Globalization;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellProviders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    /// <seealso cref="ReportColumnBuilderTests.AddGlobalPropertiesTest"/>
    public class AddGlobalPropertiesTest
    {
        [Fact]
        public void AddGlobalPropertiesShouldAddPropertiesToAllColumnsAndRows()
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();
            schemaBuilder.AddColumn("Value", new ComputedValueReportCellProvider<int, int>(x => x));
            schemaBuilder.AddColumn("As string", new ComputedValueReportCellProvider<int, string>(x => x.ToString(CultureInfo.InvariantCulture)));

            schemaBuilder.AddGlobalProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(new[]
            {
                1,
                2,
            });
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                    ReportCellHelper.CreateReportCell("As string"),
                },
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
                    ReportCellHelper.CreateReportCell(1, expectedProperties),
                    ReportCellHelper.CreateReportCell("1", expectedProperties),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell(2, expectedProperties),
                    ReportCellHelper.CreateReportCell("2", expectedProperties),
                },
            });
        }

        [Fact]
        public void AddGlobalPropertiesShouldAddPropertiesToAllColumnsAndRowsForHorizontal()
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();
            schemaBuilder.AddColumn("Value", new ComputedValueReportCellProvider<int, int>(x => x));
            schemaBuilder.AddColumn("As string", new ComputedValueReportCellProvider<int, string>(x => x.ToString(CultureInfo.InvariantCulture)));

            schemaBuilder.AddGlobalProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = schemaBuilder.BuildHorizontalSchema(0).BuildReportTable(new[]
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
