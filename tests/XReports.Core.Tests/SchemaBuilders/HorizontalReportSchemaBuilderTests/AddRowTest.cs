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
    public class AddRowTest
    {
        [Fact]
        public void AddRowShouldAddRowAtEnd()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);
            const string rowName = "TheRow";

            schemaBuilder.AddRow(rowName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "Row1" },
                new[] { "Row2" },
                new[] { rowName },
            });
        }

        [Fact]
        public void AddRowShouldAddRowWithExistingTitle()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.AddRow("Row1", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "Row1" },
                new[] { "Row2" },
                new[] { "Row1" },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void AddRowShouldAddRowWithEmptyTitle(string title)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            schemaBuilder.AddRow(title, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { title },
            });
        }

        [Fact]
        public void AddRowShouldThrowWhenTitleIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            Action action = () => schemaBuilder.AddRow(null, new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddRowShouldAddRowWithId()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);
            const string rowName = "TheRow";

            schemaBuilder.AddRow(new RowId("Row"), rowName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "Row1" },
                new[] { "Row2" },
                new[] { rowName },
            });
        }

        [Fact]
        public void AddRowShouldThrowWhenIdIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.AddRow(null, "TheRow", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddRowShouldThrowWhenIdExists()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();
            schemaBuilder.AddRow(new RowId("Row"), "Row1", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.AddRow(new RowId("Row"), "Row2", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        private HorizontalReportSchemaBuilder<int> CreateSchemaBuilder(int rowsCount)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();

            for (int i = 0; i < rowsCount; i++)
            {
                schemaBuilder.AddRow($"Row{i + 1}", new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }
    }
}
