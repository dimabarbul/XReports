using XReports.SchemaBuilders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    /// <seealso cref="ReportColumnBuilderTests.AddPropertiesTest"/>
    public class AddPropertiesTest
    {
        [Fact]
        public void AddPropertiesShouldAddPropertiesToAllRows()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildVerticalSchema().BuildReportTable(new[]
            {
                "Test",
                "Test2",
            });

            ReportCellProperty[] expectedProperties = { new CustomProperty1(), new CustomProperty2() };
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                },
            });
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Test", expectedProperties),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Test2", expectedProperties),
                },
            });
        }

        [Fact]
        public void AddPropertiesShouldAddPropertiesToAllRowsForHorizontal()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddProperties(new CustomProperty1(), new CustomProperty2());

            IReportTable<ReportCell> table = reportBuilder.BuildHorizontalSchema(0).BuildReportTable(new[]
            {
                "Test",
                "Test2",
            });

            ReportCellProperty[] expectedProperties = { new CustomProperty1(), new CustomProperty2() };
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                    ReportCellHelper.CreateReportCell("Test", expectedProperties),
                    ReportCellHelper.CreateReportCell("Test2", expectedProperties),
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
