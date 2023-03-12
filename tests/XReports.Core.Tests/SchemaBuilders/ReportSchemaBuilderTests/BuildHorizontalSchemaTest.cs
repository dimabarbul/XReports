using System;
using System.Data;
using FluentAssertions;
using XReports.Extensions;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellsProviders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    public class BuildHorizontalSchemaTest
    {
        [Fact]
        public void BuildHorizontalSchemaShouldThrowWhenNoRowsAdded()
        {
            ReportSchemaBuilder<string> schemaBuilder = new ReportSchemaBuilder<string>();

            Action action = () => schemaBuilder.BuildHorizontalSchema(0);

            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void BuildHorizontalSchemaShouldThrowWhenDataSourceIsDataReader()
        {
            ReportSchemaBuilder<IDataReader> schemaBuilder = new ReportSchemaBuilder<IDataReader>();
            schemaBuilder.AddColumn("Value", dr => dr.GetString(0));

            Action action = () => schemaBuilder.BuildHorizontalSchema(0);

            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void BuildHorizontalSchemaShouldThrowWhenHeaderRowsCountIsGreaterThanOrEqualRowsCount(int headerRowsCount)
        {
            ReportSchemaBuilder<string> schemaBuilder = new ReportSchemaBuilder<string>();
            schemaBuilder.AddColumn("Value", s => s);

            Action action = () => schemaBuilder.BuildHorizontalSchema(headerRowsCount);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void BuildHorizontalSchemaShouldThrowWhenHeaderRowsCountIsNegative()
        {
            ReportSchemaBuilder<string> schemaBuilder = new ReportSchemaBuilder<string>();
            schemaBuilder.AddColumn("Value", s => s);

            Action action = () => schemaBuilder.BuildHorizontalSchema(-1);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void BuildHorizontalSchemaShouldThrowWhenHeaderRowIsUsedInComplexHeader()
        {
            ReportSchemaBuilder<string> schemaBuilder = new ReportSchemaBuilder<string>();
            schemaBuilder.AddComplexHeader(0, "Complex Header", "Header");
            schemaBuilder.AddColumn("Header", new EmptyCellsProvider<string>());
            schemaBuilder.AddColumn("Value", s => s);

            Action action = () => schemaBuilder.BuildHorizontalSchema(1);

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void BuildHorizontalSchemaShouldMoveCorrectRowsCountToHeader()
        {
            ReportSchemaBuilder<string> schemaBuilder = new ReportSchemaBuilder<string>();
            schemaBuilder.AddComplexHeader(0, "Complex Header", "Value");
            schemaBuilder.AddColumn("Header", new EmptyCellsProvider<string>());
            schemaBuilder.AddColumn("Value", s => s);

            IReportTable<ReportCell> reportTable = schemaBuilder.BuildHorizontalSchema(1)
                .BuildReportTable(new[] { "Test", "Test2" });

            reportTable.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Header", columnSpan: 2),
                    null,
                    ReportCellHelper.CreateReportCell(string.Empty),
                    ReportCellHelper.CreateReportCell(string.Empty),
                },
            });
            reportTable.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Complex Header"),
                    ReportCellHelper.CreateReportCell("Value"),
                    ReportCellHelper.CreateReportCell("Test"),
                    ReportCellHelper.CreateReportCell("Test2"),
                },
            });
        }
    }
}
