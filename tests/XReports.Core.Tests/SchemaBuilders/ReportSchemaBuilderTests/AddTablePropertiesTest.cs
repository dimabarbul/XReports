using System;
using FluentAssertions;
using XReports.SchemaBuilders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    public class AddTablePropertiesTest
    {
        [Fact]
        public void AddTablesPropertiesShouldAddPropertiesToReportTable()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            CustomTableProperty1 tableProperty1 = new CustomTableProperty1();
            CustomTableProperty2 tableProperty2 = new CustomTableProperty2();
            reportBuilder.AddColumn("Value", s => s);
            reportBuilder.AddTableProperties(tableProperty1, tableProperty2);

            IReportTable<ReportCell> table = reportBuilder.BuildVerticalSchema().BuildReportTable(new[]
            {
                "Test",
            });

            ReportTableProperty[] expectedProperties = { tableProperty1, tableProperty2 };
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
                    ReportCellHelper.CreateReportCell("Test"),
                },
            });
            table.Properties.Should().BeEquivalentTo(expectedProperties);
        }

        [Fact]
        public void AddTablesPropertiesShouldAddPropertiesToReportTableForHorizontal()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            CustomTableProperty1 tableProperty1 = new CustomTableProperty1();
            CustomTableProperty2 tableProperty2 = new CustomTableProperty2();
            reportBuilder.AddColumn("Value", s => s);
            reportBuilder.AddTableProperties(tableProperty1, tableProperty2);

            IReportTable<ReportCell> table = reportBuilder.BuildHorizontalSchema(0).BuildReportTable(new[]
            {
                "Test",
            });

            ReportTableProperty[] expectedProperties = { tableProperty1, tableProperty2 };
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                    ReportCellHelper.CreateReportCell("Test"),
                },
            });
            table.Properties.Should().BeEquivalentTo(expectedProperties);
        }

        [Fact]
        public void AddTablesPropertiesShouldNotThrowWhenPropertyOfSameTypeAddedMultipleTimes()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            CustomTableProperty1 tableProperty1 = new CustomTableProperty1();
            CustomTableProperty1 tableProperty2 = new CustomTableProperty1();
            reportBuilder.AddColumn("Value", s => s);
            reportBuilder.AddTableProperties(tableProperty1, tableProperty2);

            IReportTable<ReportCell> table = reportBuilder.BuildVerticalSchema().BuildReportTable(new[]
            {
                "Test",
            });

            ReportTableProperty[] expectedProperties = { tableProperty1, tableProperty2 };
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
                    ReportCellHelper.CreateReportCell("Test"),
                },
            });
            table.Properties.Should().BeEquivalentTo(expectedProperties);
        }

        [Fact]
        public void AddTablesPropertiesShouldThrowWhenSomePropertyIsNull()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s);

            Action action = () => reportBuilder.AddTableProperties(new CustomTableProperty1(), new CustomTableProperty2(), null);

            action.Should().ThrowExactly<ArgumentException>();
        }

        private class CustomTableProperty1 : ReportTableProperty
        {
        }

        private class CustomTableProperty2 : ReportTableProperty
        {
        }
    }
}
