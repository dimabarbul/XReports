using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Assertions;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class HorizontalReportTest
    {
        [Fact]
        public void BuildShouldSupportHeader()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddHeaderRow(string.Empty, s => s.Length);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { string.Empty, 4 },
            });
        }

        [Fact]
        public void BuildShouldSupportMultipleHeaderRows()
        {
            HorizontalReportSchemaBuilder<string> reportBuilder = new();
            reportBuilder.AddRow("Value", s => s);
            reportBuilder.AddHeaderRow(string.Empty, s => s.Length);
            reportBuilder.AddHeaderRow(string.Empty, s => s.Substring(0, 1));

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[] { string.Empty, 4 },
                new object[] { string.Empty, "T" },
            });
        }
    }
}
