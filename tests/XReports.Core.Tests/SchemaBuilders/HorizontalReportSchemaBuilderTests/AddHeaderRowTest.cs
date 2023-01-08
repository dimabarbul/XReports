using System;
using System.Linq;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.HorizontalReportSchemaBuilderTests
{
    public class AddHeaderRowTest
    {
        [Fact]
        public void AddHeaderRowShouldAddMultipleHeaders()
        {
            HorizontalReportSchemaBuilder<string> schemaBuilder = new HorizontalReportSchemaBuilder<string>();
            schemaBuilder.AddRow("Row", new EmptyCellsProvider<string>());

            schemaBuilder.AddHeaderRow("Value", new ComputedValueReportCellsProvider<string, string>(x => x));
            schemaBuilder.AddHeaderRow("Length", new ComputedValueReportCellsProvider<string, int>(x => x.Length));

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(new[]
            {
                "Test",
                "Test2",
            });
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new object[]
                {
                    "Value",
                    "Test",
                    "Test2",
                },
                new object[]
                {
                    "Length",
                    4,
                    5,
                },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void AddHeaderRowShouldNotThrowWhenTitleIsEmpty(string title)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();
            schemaBuilder.AddRow("Row", new EmptyCellsProvider<int>());

            schemaBuilder.AddHeaderRow(title, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().BeEquivalentTo(new[]
            {
                new[] { title },
            });
        }

        [Fact]
        public void AddHeaderRowShouldThrowWhenTitleIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();

            Action action = () => schemaBuilder.AddHeaderRow(null, new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }
    }
}
