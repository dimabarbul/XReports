using System;
using System.Collections.Generic;
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
    public class InsertRowBeforeTest
    {
        [Fact]
        public void InsertRowBeforeShouldInsertRowAtCorrectPosition()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertRowBefore("Row1", "TheRow", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "TheRow" },
                new[] { "Row1" },
                new[] { "Row2" },
            });
        }

        [Fact]
        public void InsertRowBeforeShouldInsertRowWithExistingTitle()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertRowBefore("Row2", "Row1", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "Row1" },
                new[] { "Row1" },
                new[] { "Row2" },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void InsertRowBeforeShouldInsertRowWithEmptyTitle(string title)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertRowBefore("Row1", title, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { title },
                new[] { "Row1" },
                new[] { "Row2" },
            });
        }

        [Fact]
        public void InsertRowBeforeShouldThrowWhenTitleIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertRowBefore("Row1", null, new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertRowBeforeShouldThrowWhenBeforeRowNameIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertRowBefore((string)null, "TheRow", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertRowBeforeShouldThrowWhenTitleIsInDifferentCase()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertRowBefore("Row1".ToUpperInvariant(), "TheRow", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertRowBeforeShouldThrowWhenTitleDoesNotExist()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertRowBefore("RowX".ToUpperInvariant(), "TheRow", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertRowBeforeShouldInsertRowBeforeFirstOccurenceOfTitleWhenMultipleRowsHasTheTitle()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(new[] { "Row1", "Row2", "Row1" });

            schemaBuilder.InsertRowBefore("Row1", "TheRow", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "TheRow" },
                new[] { "Row1" },
                new[] { "Row2" },
                new[] { "Row1" },
            });
        }

        [Fact]
        public void InsertRowBeforeByTitleShouldInsertRowWithId()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertRowBefore("Row1", new RowId("Row"), "TheRow", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "TheRow" },
                new[] { "Row1" },
                new[] { "Row2" },
            });
        }

        [Fact]
        public void InsertRowBeforeByTitleShouldThrowWhenIdIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(1);

            Action action = () => schemaBuilder.InsertRowBefore("Row1", null, "TheRow", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertRowBeforeByTitleShouldThrowWhenIdExists()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddRow(new RowId("Row"), "Row1", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertRowBefore("Row1", new RowId("Row"), "TheRow", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertRowBeforeByIdShouldInsertRowWithId()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddRow(new RowId("1"), "Row1", new EmptyCellsProvider<int>());
            schemaBuilder.AddRow(new RowId("2"), "Row2", new EmptyCellsProvider<int>());

            schemaBuilder.InsertRowBefore(new RowId("1"), new RowId("3"), "TheRow", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "TheRow" },
                new[] { "Row1" },
                new[] { "Row2" },
            });
        }

        [Fact]
        public void InsertRowBeforeByIdShouldThrowWhenIdIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddRow(new RowId("1"), "Row1", new EmptyCellsProvider<int>());
            schemaBuilder.AddRow(new RowId("2"), "Row2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertRowBefore(new RowId("1"), null, "3", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertRowBeforeByIdShouldThrowWhenIdExists()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddRow(new RowId("1"), "Row1", new EmptyCellsProvider<int>());
            schemaBuilder.AddRow(new RowId("2"), "Row2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertRowBefore(new RowId("1"), new RowId("2"), "TheRow", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertRowBeforeByIdShouldThrowWhenBeforeIdIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddRow(new RowId("1"), "Row1", new EmptyCellsProvider<int>());
            schemaBuilder.AddRow(new RowId("2"), "Row2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertRowBefore((RowId)null, new RowId("3"), "TheRow", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertRowBeforeByIdShouldThrowWhenBeforeIdDoesNotExist()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddRow(new RowId("1"), "Row1", new EmptyCellsProvider<int>());
            schemaBuilder.AddRow(new RowId("2"), "Row2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertRowBefore(new RowId("0"), new RowId("3"), "TheRow", new EmptyCellsProvider<int>());

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

        private HorizontalReportSchemaBuilder<int> CreateSchemaBuilder(IEnumerable<string> rows)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = new HorizontalReportSchemaBuilder<int>();

            foreach (string row in rows)
            {
                schemaBuilder.AddRow(row, new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }
    }
}
