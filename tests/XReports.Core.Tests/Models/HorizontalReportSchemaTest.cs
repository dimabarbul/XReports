using System;
using XReports.Core.Tests.Extensions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.Models
{
    public class HorizontalReportSchemaTest
    {
        [Fact]
        public void ReportShouldHaveHeaderWhenThereAreNoRows()
        {
            HorizontalReportSchemaBuilder<(string FirstName, string LastName)> reportBuilder =
                new HorizontalReportSchemaBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddRow("First name", x => x.FirstName);
            reportBuilder.AddRow("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Array.Empty<(string, string)>());

            table.HeaderRows.Should().BeEmpty();
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "First name" },
                new[] { "Last name" },
            });
        }

        [Fact]
        public void EnumeratingReportMultipleTimesShouldWork()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", s => s);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "test",
            });
            // enumerating for the first time
            table.Enumerate();

            // enumerating for the second time
            table.HeaderRows.Should().BeEmpty();
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "Value", "test" },
            });
        }

        [Fact]
        public void SchemaShouldBeAvailableForBuildingMultipleReportsWithDifferentData()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder =
                new HorizontalReportSchemaBuilder<string>();
            reportBuilder.AddRow("Value", x => x);
            reportBuilder.AddRow("Length", x => x.Length);

            HorizontalReportSchema<string> schema =
                reportBuilder.BuildSchema();
            IReportTable<ReportCell> table1 = schema.BuildReportTable(new[]
            {
                "Test",
            });
            IReportTable<ReportCell> table2 = schema.BuildReportTable(new[]
            {
                "String",
            });

            table1.HeaderRows.Should().BeEmpty();
            table1.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "Value", "Test" },
                new object[] { "Length", 4 },
            });
            table2.HeaderRows.Should().BeEmpty();
            table2.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "Value", "String" },
                new object[] { "Length", 6 },
            });
        }
    }
}
