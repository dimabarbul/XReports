using System;
using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    public class AddTablePropertiesTest
    {
        [Fact]
        public void AddTablesPropertiesShouldAddPropertiesToReportTable()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            CustomTableProperty1 tableProperty1 = new CustomTableProperty1();
            CustomTableProperty2 tableProperty2 = new CustomTableProperty2();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddTableProperties(tableProperty1, tableProperty2);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
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
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            CustomTableProperty1 tableProperty1 = new CustomTableProperty1();
            CustomTableProperty1 tableProperty2 = new CustomTableProperty1();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddTableProperties(tableProperty1, tableProperty2);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
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
        public void AddTablesPropertiesShouldThrowWhenSomePropertyIsNull()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s);

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
