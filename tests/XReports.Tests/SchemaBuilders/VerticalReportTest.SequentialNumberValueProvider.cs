using FluentAssertions;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Assertions;
using XReports.ValueProviders;
using Xunit;

namespace XReports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void BuildShouldSupportSequentialNumberValueProviderWithDefaultStartValue()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "John Doe",
                "Jane Doe",
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "#" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { 1 },
                new object[] { 2 },
            });
        }

        [Fact]
        public void BuildShouldSupportSequentialNumberValueProviderWithNonDefaultStartValue()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider(15));

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "John Doe",
                "Jane Doe",
            });

            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { "#" },
            });
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { 15 },
                new object[] { 16 },
            });
        }
    }
}
