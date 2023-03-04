using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    public class InsertColumnBeforeTest
    {
        [Fact]
        public void InsertColumnBeforeByTitleShouldInsertColumnAtCorrectPosition()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column1", "TheColumn", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
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
        public void InsertColumnBeforeByTitleShouldInsertColumnWithExistingTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column2", "Column1", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void InsertColumnBeforeByTitleShouldInsertColumnWithEmptyTitle(string title)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column1", title, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(title),
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                },
            });
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldThrowWhenTitleIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore("Column1", null, new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldThrowWhenBeforeColumnNameIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore((string)null, "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldThrowWhenTitleIsInDifferentCase()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore("Column1".ToUpperInvariant(), "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldThrowWhenTitleDoesNotExist()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore("ColumnX".ToUpperInvariant(), "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldInsertColumnBeforeFirstOccurenceOfTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(new[] { "Column1", "Column2", "Column1" });

            schemaBuilder.InsertColumnBefore("Column1", "TheColumn", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("TheColumn"),
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell("Column1"),
                },
            });
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldInsertColumnWithId()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column1", new ColumnId("Column"), "TheColumn", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
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
        public void InsertColumnBeforeByTitleShouldThrowWhenIdIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(1);

            Action action = () => schemaBuilder.InsertColumnBefore("Column1", null, "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldThrowWhenIdExists()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("Column"), "Column1", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertColumnBefore("Column1", new ColumnId("Column"), "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnBeforeByIdShouldInsertColumnWithId()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellsProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellsProvider<int>());

            schemaBuilder.InsertColumnBefore(new ColumnId("1"), new ColumnId("3"), "TheColumn", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
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
        public void InsertColumnBeforeByIdShouldThrowWhenIdIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellsProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertColumnBefore(new ColumnId("1"), null, "3", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnBeforeByIdShouldThrowWhenIdExists()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellsProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertColumnBefore(new ColumnId("1"), new ColumnId("2"), "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnBeforeByIdShouldThrowWhenBeforeIdIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellsProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertColumnBefore((ColumnId)null, new ColumnId("3"), "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnBeforeByIdShouldThrowWhenBeforeIdDoesNotExist()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellsProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.InsertColumnBefore(new ColumnId("0"), new ColumnId("3"), "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        private VerticalReportSchemaBuilder<int> CreateSchemaBuilder(int columnsCount)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();

            for (int i = 0; i < columnsCount; i++)
            {
                schemaBuilder.AddColumn($"Column{i + 1}", new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }

        private VerticalReportSchemaBuilder<int> CreateSchemaBuilder(IEnumerable<string> columns)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();

            foreach (string column in columns)
            {
                schemaBuilder.AddColumn(column, new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }
    }
}
