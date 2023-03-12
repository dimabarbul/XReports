using System;
using XReports.Core.Tests.Extensions;
using XReports.Extensions;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.Models
{
    public class HorizontalReportSchemaTest
    {
        [Fact]
        public void ReportShouldHaveHeaderWhenThereAreNoRows()
        {
            ReportSchemaBuilder<(string FirstName, string LastName)> reportBuilder =
                new ReportSchemaBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddColumn("First name", x => x.FirstName);
            reportBuilder.AddColumn("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.BuildHorizontalSchema(0).BuildReportTable(Array.Empty<(string, string)>());

            table.HeaderRows.Should().BeEmpty();
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("First name"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Last name"),
                },
            });
        }

        [Fact]
        public void EnumeratingReportMultipleTimesShouldWork()
        {
            ReportSchemaBuilder<string> reportBuilder = new ReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s);

            IReportTable<ReportCell> table = reportBuilder.BuildHorizontalSchema(0).BuildReportTable(new[]
            {
                "test",
            });
            // enumerating for the first time
            table.Enumerate();

            // enumerating for the second time
            table.HeaderRows.Should().BeEmpty();
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                    ReportCellHelper.CreateReportCell("test"),
                },
            });
        }

        [Fact]
        public void SchemaShouldBeAvailableForBuildingMultipleReportsWithDifferentData()
        {
            ReportSchemaBuilder<string> reportBuilder =
                new ReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", x => x);
            reportBuilder.AddColumn("Length", x => x.Length);

            IHorizontalReportSchema<string> schema = reportBuilder.BuildHorizontalSchema(0);
            IReportTable<ReportCell> table1 = schema.BuildReportTable(new[]
            {
                "Test",
            });
            IReportTable<ReportCell> table2 = schema.BuildReportTable(new[]
            {
                "String",
            });

            table1.HeaderRows.Should().BeEmpty();
            table1.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                    ReportCellHelper.CreateReportCell("Test"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Length"),
                    ReportCellHelper.CreateReportCell(4),
                },
            });
            table2.HeaderRows.Should().BeEmpty();
            table2.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Value"),
                    ReportCellHelper.CreateReportCell("String"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Length"),
                    ReportCellHelper.CreateReportCell(6),
                },
            });
        }
    }
}
