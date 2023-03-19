using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellProviders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportSchemaBuilderTests
{
    public class InsertColumnBeforeTest
    {
        [Fact]
        public void InsertColumnBeforeByTitleShouldInsertColumnAtCorrectPosition()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column1", "TheColumn", new EmptyCellProvider<int>());

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
        public void InsertColumnBeforeByTitleShouldInsertColumnAtCorrectPositionForHorizontal()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column1", "TheColumn", new EmptyCellProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildHorizontalSchema(0).BuildReportTable(Enumerable.Empty<int>());
            table.Rows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("TheColumn"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                },
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column2"),
                },
            });
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldInsertColumnWithExistingTitle()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column2", "Column1", new EmptyCellProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());
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
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column1", title, new EmptyCellProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());
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
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore("Column1", null, new EmptyCellProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldThrowWhenBeforeColumnNameIsNull()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore((string)null, "TheColumn", new EmptyCellProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldThrowWhenTitleIsInDifferentCase()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore("Column1".ToUpperInvariant(), "TheColumn", new EmptyCellProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldThrowWhenTitleDoesNotExist()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.InsertColumnBefore("ColumnX".ToUpperInvariant(), "TheColumn", new EmptyCellProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldInsertColumnBeforeFirstOccurenceOfTitle()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(new[] { "Column1", "Column2", "Column1" });

            schemaBuilder.InsertColumnBefore("Column1", "TheColumn", new EmptyCellProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildVerticalSchema().BuildReportTable(Enumerable.Empty<int>());
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
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.InsertColumnBefore("Column1", new ColumnId("Column"), "TheColumn", new EmptyCellProvider<int>());

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
        public void InsertColumnBeforeByTitleShouldThrowWhenIdIsNull()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(1);

            Action action = () => schemaBuilder.InsertColumnBefore("Column1", null, "TheColumn", new EmptyCellProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnBeforeByTitleShouldThrowWhenIdExists()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("Column"), "Column1", new EmptyCellProvider<int>());

            Action action = () => schemaBuilder.InsertColumnBefore("Column1", new ColumnId("Column"), "TheColumn", new EmptyCellProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnBeforeByIdShouldInsertColumnWithId()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellProvider<int>());

            schemaBuilder.InsertColumnBefore(new ColumnId("1"), new ColumnId("3"), "TheColumn", new EmptyCellProvider<int>());

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
        public void InsertColumnBeforeByIdShouldThrowWhenIdIsNull()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellProvider<int>());

            Action action = () => schemaBuilder.InsertColumnBefore(new ColumnId("1"), null, "3", new EmptyCellProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnBeforeByIdShouldThrowWhenIdExists()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellProvider<int>());

            Action action = () => schemaBuilder.InsertColumnBefore(new ColumnId("1"), new ColumnId("2"), "TheColumn", new EmptyCellProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void InsertColumnBeforeByIdShouldThrowWhenBeforeIdIsNull()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellProvider<int>());

            Action action = () => schemaBuilder.InsertColumnBefore((ColumnId)null, new ColumnId("3"), "TheColumn", new EmptyCellProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void InsertColumnBeforeByIdShouldThrowWhenBeforeIdDoesNotExist()
        {
            ReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellProvider<int>());

            Action action = () => schemaBuilder.InsertColumnBefore(new ColumnId("0"), new ColumnId("3"), "TheColumn", new EmptyCellProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        private ReportSchemaBuilder<int> CreateSchemaBuilder(int columnsCount)
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();

            for (int i = 0; i < columnsCount; i++)
            {
                schemaBuilder.AddColumn($"Column{i + 1}", new EmptyCellProvider<int>());
            }

            return schemaBuilder;
        }

        private ReportSchemaBuilder<int> CreateSchemaBuilder(IEnumerable<string> columns)
        {
            ReportSchemaBuilder<int> schemaBuilder = new ReportSchemaBuilder<int>();

            foreach (string column in columns)
            {
                schemaBuilder.AddColumn(column, new EmptyCellProvider<int>());
            }

            return schemaBuilder;
        }
    }
}
