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

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertRowBeforeShouldThrowWhenBeforeRowNameIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertRowBefore(null, "TheRow", new EmptyCellsProvider<int>());

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
