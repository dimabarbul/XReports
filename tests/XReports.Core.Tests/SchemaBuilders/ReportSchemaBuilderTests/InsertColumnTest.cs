using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellsProviders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    public class InsertColumnTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InsertColumnShouldInsertColumnAtSpecifiedPosition(int index)
        {
            List<string> columns = new List<string>(new[] { "Column1", "Column2" });
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(columns);
            const string columnName = "TheColumn";

            schemaBuilder.InsertColumn(index, columnName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());
            columns.Insert(index, columnName);
            table.HeaderRows.Should().Equal(new[]
            {
                columns.Select(c => ReportCellHelper.CreateReportCell(c)).ToArray(),
            });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InsertColumnShouldInsertColumnAtSpecifiedPositionForHorizontal(int index)
        {
            List<string> columns = new List<string>(new[] { "Column1", "Column2" });
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(columns);
            const string columnName = "TheColumn";

            schemaBuilder.InsertColumn(index, columnName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildHorizontalSchema(0).BuildReportTable(Enumerable.Empty<int>());
            columns.Insert(index, columnName);
            table.Rows.Should().Equal(
                columns.Select(c => new[] { ReportCellHelper.CreateReportCell(c) }).ToArray());
        }

        [Fact]
        public void InsertColumnShouldInsertColumnWithExistingTitle()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumn(0, "Column2", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void InsertColumnShouldInsertColumnWithEmptyTitle(string title)
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            schemaBuilder.InsertColumn(0, title, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(title),
                },
            });
        }

        [Fact]
        public void InsertColumnShouldThrowWhenTitleIsNull()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(1);

            Action action = () => schemaBuilder.InsertColumn(0, null, new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnShouldThrowWhenIndexIsGreaterThanColumnCount()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumn(3, "Column", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void InsertColumnShouldThrowWhenIndexIsLessThanZero()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumn(-1, "Column", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void InsertColumnShouldInsertColumnWithId()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(new[] { "Column1", "Column2" });

            schemaBuilder.InsertColumn(0, new ColumnId("Column"), "TheColumn", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("TheColumn"),
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                },
            });
        }

        [Fact]
        public void InsertColumnShouldThrowWhenIdIsNull()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumn(0, null, "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnShouldThrowWhenIdExists()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("Column"), "Column1", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertColumn(0, new ColumnId("Column"), "Column2", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        private ReportSchemaBuilder<int> CreateSchemaBuilder(int columnsCount)
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();

            for (int i = 0; i < columnsCount; i++)
            {
                schemaBuilder.AddColumn($"Column{i + 1}", new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }

        private ReportSchemaBuilder<int> CreateSchemaBuilder(IEnumerable<string> columns)
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();

            foreach (string column in columns)
            {
                schemaBuilder.AddColumn(column, new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }
    }
}
