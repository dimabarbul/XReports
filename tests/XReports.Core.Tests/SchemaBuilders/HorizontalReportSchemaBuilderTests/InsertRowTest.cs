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
    public class InsertRowTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InsertRowShouldInsertRowAtSpecifiedPosition(int index)
        {
            List<string> rows = new List<string>(new[] { "Row1", "Row2" });
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(rows);
            const string rowsName = "TheRow";

            schemaBuilder.InsertRow(index, rowsName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            rows.Insert(index, rowsName);
            table.Rows.Should().BeEquivalentTo(rows
                .Select(row => new object[] { row })
                .ToArray());
        }

        [Fact]
        public void InsertRowShouldInsertRowWithExistingTitle()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertRow(0, "Row2", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "Row2" },
                new[] { "Row1" },
                new[] { "Row2" },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void InsertRowShouldInsertRowWithEmptyTitle(string title)
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            schemaBuilder.InsertRow(0, title, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { title },
            });
        }

        [Fact]
        public void InsertRowShouldThrowWhenTitleIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(1);

            Action action = () => schemaBuilder.InsertRow(0, null, new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertRowShouldThrowWhenIndexIsGreaterThanRowCount()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertRow(3, "Row", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void InsertRowShouldThrowWhenIndexIsLessThanZero()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertRow(-1, "Row", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void InsertRowShouldInsertRowWithId()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(new[] { "Row1", "Row2" });

            schemaBuilder.InsertRow(0, new RowId("Row"), "TheRow", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new object[] { "TheRow" },
                new object[] { "Row1" },
                new object[] { "Row2" },
            });
        }

        [Fact]
        public void InsertRowShouldThrowWhenIdIsNull()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertRow(0, null, "TheRow", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertRowShouldThrowWhenIdExists()
        {
            HorizontalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddRow(new RowId("Row"), "Row1", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertRow(0, new RowId("Row"), "Row2", new EmptyCellsProvider<int>());

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
