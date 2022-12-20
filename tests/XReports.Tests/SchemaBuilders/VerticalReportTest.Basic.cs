using System;
using System.Collections.Generic;
using System.Linq;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void BuildShouldHaveHeaderWhenNoRows()
        {
            VerticalReportSchemaBuilder<(string FirstName, string LastName)> reportBuilder = new();
            reportBuilder.AddColumn("First name", x => x.FirstName);
            reportBuilder.AddColumn("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(Array.Empty<(string, string)>());

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new [] { "First name", "Last name" },
            });
            table.Rows.Should().BeEquivalentTo(Enumerable.Empty<IEnumerable<object>>());
        }

        [Fact]
        public void BuildShouldSupportMultipleRows()
        {
            VerticalReportSchemaBuilder<(string FirstName, string LastName)> reportBuilder = new();
            reportBuilder.AddColumn("First name", x => x.FirstName);
            reportBuilder.AddColumn("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                ("John", "Doe"),
                ("Jane", "Do"),
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "First name", "Last name" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "John", "Doe" },
                new[] { "Jane", "Do" },
            });
        }

        [Fact]
        public void EnumeratingReportMultipleTimesShouldWork()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new();
            reportBuilder.AddColumn("Value", s => s);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "test",
            });
            // enumerating for the first time
            foreach (IEnumerable<ReportCell> row in table.HeaderRows)
            {
                foreach (ReportCell _ in row)
                {
                }
            }
            foreach (IEnumerable<ReportCell> row in table.Rows)
            {
                foreach (ReportCell _ in row)
                {
                }
            }

            // enumerating for the second time
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "Value" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "test" },
            });
        }
    }
}
